using Cacx.LanguageManager.Core;
using Cacx.LanguageManager.Wpf;
using System.Globalization;
using System.Windows;

namespace LanguageManagerSample;
/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public App()
    {
        LocalizationService localizationService = new("Cacx.LanguageManager.SampleOne.Resources.Welcome");
        localizationService.SetLanguage(new CultureInfo("de-DE"));
        LocalizationProvider.Service = localizationService;
    }
}

