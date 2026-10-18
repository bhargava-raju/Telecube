using Cart.Application.Abstractions;
using Cart.Infrastructure.Clients;
using Cart.Infrastructure.Messaging;
using Cart.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace Cart.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddCartInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var redisConnectionString = configuration.GetConnectionString("Redis")
            ?? throw new InvalidOperationException("Missing connection string 'Redis'.");
        var catalogBaseUrl = configuration["Services:Catalog:BaseUrl"]
            ?? throw new InvalidOperationException("Missing setting 'Services:Catalog:BaseUrl'.");

        // One multiplexer for the whole app: it manages its own connection pool and is thread-safe.
        services.AddSingleton<IConnectionMultiplexer>(_ => ConnectionMultiplexer.Connect(redisConnectionString));

        services.AddHttpClient<ICatalogClient, CatalogClient>(client => client.BaseAddress = new Uri(catalogBaseUrl))
            .AddStandardResilienceHandler();

        services.AddScoped<IIntegrationEventPublisher, MassTransitIntegrationEventPublisher>();
        services.AddScoped<ICartRepository, RedisCartRepository>();

        return services;
    }
}
