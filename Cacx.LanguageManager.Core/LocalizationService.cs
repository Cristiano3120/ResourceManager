using Cacx.LanguageManager.Abstractions;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Cacx.LanguageManager.Core;

/// <summary>
/// Provides localization services for retrieving culture-specific resources, strings, and objects for an application.
/// </summary>
/// <remarks>The LocalizationService manages the current culture and resource context, enabling dynamic language
/// switching and resource retrieval at runtime. It supports updating the resource manager's base path to accommodate
/// modular resource folder structures and raises property change notifications to facilitate UI updates when the
/// culture changes. The service is designed to work seamlessly with WPF data binding and design-time
/// scenarios.</remarks>
public sealed class LocalizationService : ILocalizationService
{
    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    /// <remarks>
    /// UI elements and data bindings can subscribe to this event to refresh their displayed values when the language changes
    /// </remarks>
    public event PropertyChangedEventHandler? PropertyChanged;
    private ResourceManager _resourceManager;
    private CultureInfo _currentCulture;

    /// <summary>
    /// Initializes a new instance of the LocalizationService class using the specified resource base path and culture
    /// information.
    /// </summary>
    /// <remarks>Use this constructor to specify a custom culture for localization, or to target a specific
    /// resource set. If no culture is provided, the service defaults to the application's current UI culture.</remarks>
    /// <param name="basePath">The base name of the resource file to use for localization. This should correspond to the root name of the .resx
    /// files containing localized strings.</param>
    /// <param name="cultureInfo">The culture information to use for resource lookup. If null, the current UI culture is used.</param>
    public LocalizationService(string basePath, CultureInfo? cultureInfo = null) 
    {
        LocalizationProvider.Service = this;
        _currentCulture = cultureInfo ?? CultureInfo.CurrentUICulture;
        _resourceManager = new ResourceManager(basePath, Assembly.GetCallingAssembly());
    }

    /// <summary>
    /// Sets the current language and culture for the instance.
    /// </summary>
    /// <remarks>Calling this method updates the <c>CurrentCulture</c> property and raises the
    /// <c>PropertyChanged</c> event for all properties. This allows data bindings and UI elements to refresh and
    /// reflect the new culture settings.</remarks>
    /// <param name="cultureInfo">The <see cref="CultureInfo"/> to use as the current language and culture. Cannot be null.</param>
    public void SetLanguage(CultureInfo cultureInfo)
    {
        _currentCulture = cultureInfo;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
    }

    /// <summary>
    /// Call this method when you want the resource manager to point to a different base path.
    /// This is needed if you are working with this kind of folder structure(recommended):
    /// <code>
    /// MyApp(Project)
    ///    ->Resources(Folder)
    ///        ->Login(Folder)
    ///            Login.resx
    ///            Login.de-DE.resx
    ///        ->CreateAccount(Folder)
    ///            CreateAccount.resx
    ///            CreateAccount.de-DE.resx
    /// </code>
    /// 
    /// <c>If you wanna switch from the Login to the CreateAccount resources, you need to call this method</c>
    /// </summary>
    /// 
    /// <param name="basePath">Example path for the folder structure shown in the summary:
    /// <c> MyApp.Resources.Login.Login</c>
    /// </param>
    public void UpdateContext(string basePath)
    {
        _resourceManager = new ResourceManager(basePath, Assembly.GetCallingAssembly());
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
    }

    /// <summary>
    /// Gets the current language or culture used by the application.
    /// </summary>
    /// <returns>A <see cref="CultureInfo"/> object representing the application's current culture.</returns>
    public CultureInfo GetLanguage() => _currentCulture;

    /// <summary>
    /// Retrieves the localized string associated with the specified resource key.
    /// </summary>
    /// <remarks>If the method is invoked by the WPF Designer, the key is returned directly instead of a
    /// localized value. When a resource is missing, the returned string includes the resource manager's base name to
    /// aid in debugging missing keys.</remarks>
    /// <param name="key">The resource key for which to retrieve the localized string. Cannot be null.</param>
    /// <returns>The localized string corresponding to the specified key. If the key is not found, returns a string in the format
    /// "MissingKey:{BaseName}". If called by the WPF Designer, returns the key itself.</returns>
    public string GetString(string key)
    {
        // "" means that the WPF Designer called this method!
        if (_resourceManager.BaseName == "")
        {
            return key;
        }
        
        return _resourceManager.GetString(key, _currentCulture) 
            ?? $"MissingKey:{_resourceManager.BaseName}";
    }

    /// <summary>
    /// Retrieves the resource object associated with the specified key from the resource manager.
    /// </summary>
    /// <remarks>If the resource manager's base name is empty, indicating a call from the WPF Designer, the
    /// method returns the key directly. Otherwise, if the resource is not found, the return value is a string in the
    /// format "MissingKey:{BaseName}".</remarks>
    /// <param name="key">The key that identifies the resource object to retrieve. Cannot be null.</param>
    /// <returns>The resource object associated with the specified key, or a string indicating a missing key if the resource is
    /// not found. If called by the WPF Designer, returns the key itself.</returns>
    public object GetObject(string key)
    {
        // "" means that the WPF Designer called this method!
        if (_resourceManager.BaseName == "")
        {
            return key;
        }

        return _resourceManager.GetObject(key, _currentCulture) 
            ?? $"MissingKey:{_resourceManager.BaseName}";
    }

    /// <summary>
    /// Retrieves a resource stream associated with the specified key from the resource manager.
    /// </summary>
    /// <remarks>If the resource manager's base name is empty, indicating a design-time context such as the
    /// WPF Designer, the method returns <see cref="Stream.Null"/>. The returned stream may be empty if the resource is
    /// not found.</remarks>
    /// <param name="key">The key identifying the resource to retrieve. Cannot be null.</param>
    /// <returns>A <see cref="Stream"/> containing the resource data if found; otherwise, <see cref="Stream.Null"/>.</returns>
    public Stream GetStream(string key)
    {
        // "" means that the WPF Designer called this method!
        if (_resourceManager.BaseName == "")
        {
            return Stream.Null;
        }

        return _resourceManager.GetStream(key, _currentCulture) 
            ?? Stream.Null;
    }
}
