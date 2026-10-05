using Customer.Application.Abstractions;
using MassTransit;
using Telecube.Contracts;

namespace Customer.Infrastructure.Messaging;

public class MassTransitIntegrationEventPublisher(IPublishEndpoint publishEndpoint)
    : IIntegrationEventPublisher
{
    public Task PublishAsync<TEvent>(TEvent integrationEvent, CancellationToken cancellationToken)
        where TEvent : class, IIntegrationEvent
    {
        return publishEndpoint.Publish(integrationEvent, cancellationToken);
    }
}
