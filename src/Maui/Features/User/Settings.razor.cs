using TabletopPowerTools.Core.Features.Settings;
using Microsoft.JSInterop;

namespace TabletopPowerTools.Maui.Features.User;

public partial class Settings : IDisposable
{
    [Inject] private UserSettingsProvider UserSettingsProvider { get; set; }
    [Inject] private IMediator Mediator { get; set; }
    [Inject] private IJSRuntime JSRuntime { get; set; } = null!;

    private IJSObjectReference _blazorDownloadFileModule;
    private readonly CancellationTokenSource _cts = new();

    private string GetThemeIcon() => UserSettingsProvider.PrefersDarkMode ? Icons.Material.Filled.DarkMode : Icons.Material.Filled.LightMode;

    private async Task OnExportAllCreatureDataClickedAsync()
    {
        var response = await Mediator.Send(new ExportCreatureDataQuery(), _cts.Token);

        await _blazorDownloadFileModule.InvokeVoidAsync("BlazorDownloadFile", _cts.Token, response.FileName, "text/json", response.ExportedCreatureFileBytes);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _blazorDownloadFileModule = await JSRuntime.InvokeAsync<IJSObjectReference>("import", "./Features/User/Settings.razor.js");
        }
    }

    public void Dispose()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
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
