using Cacx.LanguageManager.Core;
using System.Globalization;

namespace Cacx.LanguageManager.Tests;

public class LocalizationTests
{
    private const string ResourceBasePathToLogin = "Cacx.LanguageManager.Tests.Resources.Login.Login";
    private const string ResourceBasePathToCreateAcc = "Cacx.LanguageManager.Tests.Resources.CreateAcc.CreateAcc";

    [Fact]
    public void Login_Should_Load_German_Resources()
    {
        CultureInfo culture = new("de-DE");
        LocalizationService localizationService = new(ResourceBasePathToLogin, culture);

        string result = localizationService.GetString("Default");

        Assert.Equal("GERMANY", result);
    }
    
    [Fact]
    public void Login_Should_Fallback_To_Default_Resources()
    {
        CultureInfo culture = new("fr-FR"); //French culture which does not exist in resources
        LocalizationService localizationService = new(ResourceBasePathToLogin, culture);

        string result = localizationService.GetString("Default");

        Assert.Equal("DEFAULT", result);
    }

    [Fact]
    public void Switch_To_Another_Environment_Should_Work()
    {
        CultureInfo culture = new("de-DE");
        LocalizationService localizationService = new(ResourceBasePathToLogin, culture);

        string loginResult = localizationService.GetString("Default");
        Assert.Equal("GERMANY", loginResult);

        localizationService.UpdateContext(ResourceBasePathToCreateAcc);
        string createAccResult = localizationService.GetString("Default");

        Assert.Equal("CREATE ACC GERMAN DEFAULT", createAccResult);
    }

    [Fact]
    public void Switching_Language_Should_Work()
    {
        CultureInfo culture = new("de-DE");
        LocalizationService localizationService = new(ResourceBasePathToLogin, culture);

        string germanResult = localizationService.GetString("Default");
        Assert.Equal("GERMANY", germanResult);

        localizationService.SetLanguage(new CultureInfo("en-US"));

        string usResult = localizationService.GetString("Default");
        Assert.Equal("UNITED STATES", usResult);
    }

    [Fact]
    public void Empty_BasePath_Should_Return_Entered_Key()
    {
        CultureInfo culture = new("de-DE");
        LocalizationService localizationService = new("", culture); //BasePath is gonna be empty when in design mode

        string result = localizationService.GetString("AnyKey");
        Assert.Equal("AnyKey", result);
    }
}
