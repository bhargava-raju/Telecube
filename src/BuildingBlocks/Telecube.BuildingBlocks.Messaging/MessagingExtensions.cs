using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Telecube.Contracts;

namespace Telecube.BuildingBlocks.Messaging;

public static class MessagingExtensions
{
    // For services that only publish events.
    public static IServiceCollection AddMessaging(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IServiceBusBusFactoryConfigurator>? configureTopology = null)
    {
        return services.AddMessaging(configuration, _ => { }, (_, factoryConfigurator) => configureTopology?.Invoke(factoryConfigurator));
    }

    // For services that also consume events: register the consumers, then attach them to subscriptions.
    // The bus context is passed in because ConfigureConsumer needs it to resolve consumers from DI.
    public static IServiceCollection AddMessaging(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<IBusRegistrationConfigurator> configureConsumers,
        Action<IBusRegistrationContext, IServiceBusBusFactoryConfigurator> configureTopology)
    {
        services.AddMassTransit(busConfigurator =>
        {
            configureConsumers(busConfigurator);

            busConfigurator.UsingAzureServiceBus((context, factoryConfigurator) =>
            {
                factoryConfigurator.Host(configuration.GetConnectionString("ServiceBus"));
                factoryConfigurator.Publish<IIntegrationEvent>(x => x.Exclude = true);
                configureTopology(context, factoryConfigurator);
            });
        });

        return services;
    }
}
