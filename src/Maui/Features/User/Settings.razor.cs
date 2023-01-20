namespace DMPowerTools.Maui.Features.User;

public partial class Settings
{
    [Inject] private UserSettingsProvider UserSettingsProvider { get; set; }

    private string GetThemeIcon() => UserSettingsProvider.PrefersDarkMode ? Icons.Material.Filled.DarkMode : Icons.Material.Filled.LightMode;
}

public class UserSettingsProvider
{
    private static readonly string[] _headerFontFamily = new[] { "Cormorant" };

    public Action OnUserSettingsChanged { get; set; }

    public MudThemeProvider MudThemeProvider { get; set; }

    public readonly MudTheme Theme = new()
    {
        Typography = new Typography
        {
            Default = new Default { FontFamily = new[] { "CrimsonText" }, FontSize = "1rem" },
            H1 = new H1 { FontFamily = _headerFontFamily },
            H2 = new H2 { FontFamily = _headerFontFamily },
            H3 = new H3 { FontFamily = _headerFontFamily },
            H4 = new H4 { FontFamily = _headerFontFamily },
            H5 = new H5 { FontFamily = _headerFontFamily },
            H6 = new H6 { FontFamily = _headerFontFamily }
        }
    };

    public bool PrefersDarkMode
    {
        get => Preferences.Get(nameof(PrefersDarkMode), true);
        set => Preferences.Set(nameof(PrefersDarkMode), value);
    }
}
