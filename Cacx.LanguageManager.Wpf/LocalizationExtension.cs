using System.Windows.Data;
using System.Windows.Markup;

namespace Cacx.LanguageManager.Wpf;

/// <summary>
/// Provides a markup extension for binding localized resources in XAML using a specified resource key.
/// </summary>
/// <remarks>Use this extension in XAML to bind UI elements to localized strings by specifying the resource key.
/// The extension creates a binding to the localization source, enabling dynamic updates when the application's language
/// changes.
/// 
/// Example usage in XAML:
/// <code>
/// First define the XMLNS namespace:
///     xmlns:loc="clr-namespace:Cacx.LanguageManager.Wpf;assembly=Cacx.LanguageManager.Wpf"
///     
/// Then use the LocalizationExtension on a TextBlock via a binding:
///     Text="{loc:Localization Default}"
/// </code>
/// </remarks>
/// <param name="key">The resource key used to identify the localized value to bind.</param>
public class LocalizationExtension(string key) : MarkupExtension
{
    public string Key { get; init; } = key;

    /// <summary>
    /// Returns an object that provides the localized value for the specified key in XAML markup.
    /// <para>
    /// DO NOT CALL THIS METHOD DIRECTLY. It is intended to be called by the XAML parser.
    /// </para>
    /// </summary>
    /// <remarks>This method is called by the XAML parser during object initialization to resolve
    /// localization bindings. The returned value enables dynamic localization based on the specified key.</remarks>
    /// <param name="serviceProvider">An object that provides services for the markup extension. Typically used to access contextual information about
    /// the target property and object.</param>
    /// <returns>An object representing the localized value to be set on the target property. The returned value is suitable for
    /// use in XAML bindings.</returns>
    public override object ProvideValue(IServiceProvider serviceProvider)
    {
        Binding binding = new($"[{Key}]")
        {
            Source = new LocalizationBindingSource(),
        };

        return binding.ProvideValue(serviceProvider);
    }
}
