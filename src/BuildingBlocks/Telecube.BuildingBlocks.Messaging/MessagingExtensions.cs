using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telecube.Contracts;

namespace Telecube.BuildingBlocks.Messaging;

public static class MessagingExtensions
{
    public static IServiceCollection AddMessaging(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IServiceBusBusFactoryConfigurator>? configureTopology = null)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.UsingAzureServiceBus((context, factoryConfigurator) =>
            {
                factoryConfigurator.Host(configuration.GetConnectionString("ServiceBus"));
                factoryConfigurator.Publish<IIntegrationEvent>(x => x.Exclude = true);
                configureTopology?.Invoke(factoryConfigurator);
            });
        });

        return services;
    }
}
