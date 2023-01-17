using DMPowerTools.Core.Features.Combat;
using DMPowerTools.Core.Infrastructure;
using DMPowerTools.Core.Models.Imports;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MudBlazor.Services;

namespace DMPowerTools.Maui;

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
        services.AddMediatR(typeof(ManageCombatQueryHandler));
        services.AddAutoMapper(typeof(ManageCombatQueryResponse));
        services.AddAutoMapper(typeof(TetraCubeCreature));
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(_configuration.GetConnectionString("DMPowerTools")));
    }
}
