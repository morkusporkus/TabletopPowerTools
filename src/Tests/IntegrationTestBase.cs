using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TabletopPowerTools.Core.Infrastructure;
using TabletopPowerTools.Maui;

namespace TabletopPowerTools.Tests;

[Collection(nameof(Maui))]
public class IntegrationTestBase : IAsyncLifetime
{
    public async Task InitializeAsync() => await ResetStateAsync();

    public Task DisposeAsync() => Task.CompletedTask;

    public static T GetRequiredService<T>() where T : class =>
        _serviceProvider.GetRequiredService<T>();
}

[CollectionDefinition(nameof(Maui))]
public class IntegrationTesting : ICollectionFixture<IntegrationTesting>, IAsyncLifetime
{
    internal static IServiceProvider _serviceProvider = null!;

    public async Task InitializeAsync()
    {
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
        var startup = new Startup(configuration);
        var services = new ServiceCollection();
        startup.ConfigureServices(services);

        _serviceProvider = services.BuildServiceProvider();

        await Task.CompletedTask;
    }

    public Task DisposeAsync() => Task.CompletedTask;

    public static async Task ResetStateAsync()
    {
        using var scope = _serviceProvider.CreateScope();

        var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        await dbContext.Database.EnsureDeletedAsync();
        await dbContext.Database.EnsureCreatedAsync();
    }

    public static async Task AddAsync<TEntity>(TEntity entity)
        where TEntity : class
    {
        using var scope = _serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        context.Add(entity);

        await context.SaveChangesAsync();
    }

    public static async Task<TEntity?> FirstOrDefaultAsync<TEntity>()
        where TEntity : class
    {
        using var scope = _serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();

        return await context.Set<TEntity>().FirstOrDefaultAsync();
    }

    public static async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
    {
        using var scope = _serviceProvider.CreateScope();

        var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

        return await mediator.Send(request);
    }
}