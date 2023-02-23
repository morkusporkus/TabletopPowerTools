using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MudBlazor.Services;
using TabletopPowerTools.Core.Features.Combat.Manage;
using TabletopPowerTools.Core.Features.Creatures.Import;
using TabletopPowerTools.Core.Infrastructure;
using TabletopPowerTools.Maui.Features.User;

namespace TabletopPowerTools.Maui;

public class Startup
{
    private readonly IConfiguration _configuration;

    public Startup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
        services.AddMudServices();
        services.AddMediatR(typeof(CreatureSearchQueryHandler));
        services.AddAutoMapper(typeof(CreatureSearchQueryHandler));
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            options.UseSqlite(_configuration.GetConnectionString("TabletopPowerTools"));
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddSingleton<IUploadStrategy, TetraCubeUploadStrategy>();
        services.AddSingleton<IUploadStrategy, TableTopPowerToolsUploadStrategy>();
        services.AddSingleton<IUploadStrategyFactory, UploadStrategyFactory>();

        services.AddSingleton<UserSettingsProvider>();
    }
}
