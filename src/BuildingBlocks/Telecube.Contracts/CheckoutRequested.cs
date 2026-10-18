namespace Telecube.Contracts;

// Published by Cart when the shopper checks out. Items carry the cart's price snapshot, so Checkout never calls Catalog.
public sealed record CheckoutRequested(Guid CustomerId, IReadOnlyList<CheckoutRequestedItem> Items, decimal Total) : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; init; } = DateTimeOffset.UtcNow;
}
