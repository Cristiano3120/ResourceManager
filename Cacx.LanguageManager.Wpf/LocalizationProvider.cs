using Cacx.LanguageManager.Abstractions;
using Cacx.LanguageManager.Core;

namespace Cacx.LanguageManager.Wpf;

/// <summary>
/// Provides access to the application's current localization service instance.
/// </summary>
/// <remarks>The <see cref="Service"/> property allows getting or setting the global localization service used
/// throughout the application. By default, if no service is set, a new instance of <see cref="LocalizationService"/> is
/// created with an empty culture name. 
/// This property should be set to a new instance at the start of the application
/// as this "empty" instance is only for WPF designer purposes
/// 
/// <code>
/// string basePath = "MyApp.Resources.Login.Login";
/// LocalizationProvider.Service = new(basePath);
/// </code>
/// </remarks>
public static class LocalizationProvider
{
    /// <summary>
    /// The application's current localization service instance.
    /// </summary>
    /// <remarks>
    /// Either an instance of <see cref="LocalizationService"/> or an user defined implementation of <see cref="ILocalizationService"/>.
    /// </remarks>

    public static ILocalizationService Service
    {
        get 
        {
            field ??= new LocalizationService("");
            return field;
        }

        set => field = value;
    }
}
