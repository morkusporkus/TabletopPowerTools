using DMPowerTools.Maui;
using DMPowerTools.Maui.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DMPowerTools.Tests;

[Collection(nameof(Maui))]
public class IntegrationTestBase : IAsyncLifetime
{
    public Task DisposeAsync() => Task.CompletedTask;

    public async Task InitializeAsync() => await ResetStateAsync();
}

[CollectionDefinition(nameof(Maui))]
public class IntegrationTesting : ICollectionFixture<IntegrationTesting>, IAsyncLifetime
{
    private static IServiceScope _serviceScope = null!;

    public async Task InitializeAsync()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        var startup = new Startup(configuration);
        var services = new ServiceCollection();
        startup.ConfigureServices(services);

        var serviceProvider = services.BuildServiceProvider();
        _serviceScope = serviceProvider.CreateScope();

        await Task.CompletedTask;
    }

    public async Task DisposeAsync()
    {
        _serviceScope.Dispose();

        await Task.CompletedTask;
    }

    public static async Task ResetStateAsync()
    {
        var dbContext = _serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
    }

}