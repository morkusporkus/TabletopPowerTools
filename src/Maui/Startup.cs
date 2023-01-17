using DMPowerTools.Core.Features.Combat.Manage;
using DMPowerTools.Core.Infrastructure;
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
        services.AddMediatR(typeof(CreatureSearchQueryHandler));
        services.AddAutoMapper(typeof(CreatureSearchQueryResponse));
        services.AddDbContext<ApplicationDbContext>(options => options.UseSqlite(_configuration.GetConnectionString("DMPowerTools")));
    }
}
