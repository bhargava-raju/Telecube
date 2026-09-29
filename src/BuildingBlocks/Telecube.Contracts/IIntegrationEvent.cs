namespace Telecube.Contracts;

// Marker for cross-service integration events (as opposed to BuildingBlocks.Common.IDomainEvent,
// which stays internal to a single service's aggregate). Real event records (ProductCreated,
// CheckoutStarted, etc.) land here as each phase needs them — empty until then.
public interface IIntegrationEvent
{
    Guid EventId { get; }
    DateTimeOffset OccurredOn { get; }
}
