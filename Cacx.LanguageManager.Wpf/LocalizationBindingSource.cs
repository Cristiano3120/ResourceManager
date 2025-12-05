using Cacx.LanguageManager.Core;
using System.ComponentModel;

namespace Cacx.LanguageManager.Wpf;

/// <summary>
/// Used as a binding source for localization in WPF. Notifies when the localization changes. 
/// Instantiated by the <see cref="LocalizationExtension"/>
/// </summary>
public class LocalizationBindingSource : INotifyPropertyChanged
{
    /// <summary>
    /// Occurs when a property value changes.
    /// </summary>
    /// <remarks>
    /// This event is typically raised by the implementation of the <see
    /// cref="INotifyPropertyChanged"/> interface to notify listeners that a property value has changed. Subscribers can
    /// use this event to update UI elements or perform other actions in response to property changes.
    /// </remarks>
    public event PropertyChangedEventHandler? PropertyChanged;
    
    /// <summary>
    /// Gets the localized string associated with the specified key.
    /// </summary>
    /// <param name="key">The key that identifies the localized string to retrieve. Cannot be null.</param>
    /// <returns>The localized string corresponding to the specified key, or null if the key does not exist.</returns>
    public string this[string key] => LocalizationProvider.Service.GetString(key);

    internal LocalizationBindingSource()
    {
        LocalizationProvider.Service.PropertyChanged += (_, __) =>
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(null));
        };
    }
}
