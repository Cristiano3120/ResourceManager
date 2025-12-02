using System.ComponentModel;

namespace Cacx.LanguageManager.Wpf;

/// <summary>
/// Used as a binding source for localization in WPF. Notifies when the localization changes. 
/// Instantiated by the <see cref="LocalizationExtension"/>
/// </summary>
public class LocalizationBindingSource : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler? PropertyChanged;
    public string this[string key] => LocalizationProvider.Service.GetString(key);

    public LocalizationBindingSource()
    {
        LocalizationProvider.Service.PropertyChanged += (_, __) =>
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        };
    }
}
