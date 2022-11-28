using Microsoft.Extensions.Logging;
using DMPowerTools.Data;
using MudBlazor.Services;

namespace DMPowerTools;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();
        var dbPath =
            Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                @"DMPowerTools.db");
        builder.Services.AddSingleton<MonsterService>(
            s => ActivatorUtilities.CreateInstance<MonsterService>(s, dbPath));
#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif
        builder.Services.AddSingleton<MonsterItemDatabase>();
        //builder.Services.AddSingleton<MonsterService>();
        builder.Services.AddMudServices();
        return builder.Build();
	}
}
