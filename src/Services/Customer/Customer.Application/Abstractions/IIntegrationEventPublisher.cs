using Telecube.Contracts;

namespace Customer.Application.Abstractions;

public interface IIntegrationEventPublisher
{
    // TODO: Publish the given integration event to the message bus.
    Task PublishAsync<TEvent>(TEvent integrationEvent, CancellationToken cancellationToken)
        where TEvent : class, IIntegrationEvent;
}
