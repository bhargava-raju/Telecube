using Customer.Application.Abstractions;
using Customer.Infrastructure.Messaging;
using Customer.Infrastructure.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Customer.Infrastructure;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddCustomerInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("CustomerDb")
            ?? throw new InvalidOperationException("Missing connection string 'CustomerDb'.");

        services.AddSingleton<ISqlConnectionFactory>(new SqlConnectionFactory(connectionString));
        services.AddSingleton<IIntegrationEventPublisher, MassTransitIntegrationEventPublisher>();
        services.AddScoped<ICustomerRepository, CustomerRepository>();

        return services;
    }
}
