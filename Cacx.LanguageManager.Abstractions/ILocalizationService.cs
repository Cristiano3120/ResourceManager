using System.ComponentModel;
using System.Globalization;

namespace Cacx.LanguageManager.Abstractions;

/// <summary>
/// The localization service interface. It provides basic methods that are needed for localization.
/// You can implement your own localization service by implementing this interface.
/// </summary>
public interface ILocalizationService : INotifyPropertyChanged  
{
    public void SetLanguage(CultureInfo cultureInfo);
    public void UpdateContext(string basePath);
    public string GetString(string key);
    public CultureInfo GetLanguage();
}
