using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using TabletopPowerTools.Core.Infrastructure;

namespace TabletopPowerTools.Maui;

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

        // Non-test configuration/services.
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        var dbPath = Path.Join(path, "TabletopPowerTools.db");
        builder.Configuration.AddInMemoryCollection(new List<KeyValuePair<string, string>>
        {
            new("ConnectionStrings:TabletopPowerTools", "DataSource=" + dbPath)
        });
        builder.Services.AddMauiBlazorWebView();

        var startup = new Startup(builder.Configuration);
        startup.ConfigureServices(builder.Services);

#if DEBUG
        builder.Services.AddBlazorWebViewDeveloperTools();
        builder.Logging.AddDebug();
#endif

        var app = builder.Build();

        using var scope = app.Services.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        dbContext.Database.EnsureDeleted();
        dbContext.Database.EnsureCreated();

        return app;
    }
}
