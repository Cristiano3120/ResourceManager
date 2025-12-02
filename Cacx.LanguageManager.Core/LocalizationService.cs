using Cacx.LanguageManager.Abstractions;
using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Resources;

namespace Cacx.LanguageManager.Core;

public sealed class LocalizationService : ILocalizationService
{
    public CultureInfo CurrentCulture { get; private set; }
    public event PropertyChangedEventHandler? PropertyChanged;

    private ResourceManager _resourceManager;

    public LocalizationService(string basePath, CultureInfo? cultureInfo = null) 
    {
        CurrentCulture = cultureInfo ?? CultureInfo.CurrentUICulture;
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
        CurrentCulture = cultureInfo;
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
    }

    /// <summary>
    /// Gets the current language or culture used by the application.
    /// </summary>
    /// <returns>A <see cref="CultureInfo"/> object representing the application's current culture.</returns>
    public CultureInfo GetLanguage() => CurrentCulture;

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

        return _resourceManager.GetString(key, CurrentCulture) 
            ?? $"MissingKey:{_resourceManager.BaseName}";
    }
}
