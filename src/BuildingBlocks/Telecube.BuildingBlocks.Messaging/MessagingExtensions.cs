using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Telecube.BuildingBlocks.Messaging;

public static class MessagingExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.UsingAzureServiceBus((context, factoryConfigurator) =>
            {
                factoryConfigurator.Host(configuration.GetConnectionString("ServiceBus"));
            });
        });

        return services;
    }
}
