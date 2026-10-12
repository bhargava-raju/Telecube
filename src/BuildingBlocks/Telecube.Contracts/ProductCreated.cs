namespace Telecube.Contracts;

public sealed record ProductCreated(Guid ProductId, Guid CategoryId, string Sku, string Name, string Description, decimal Price) : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; init; } = DateTimeOffset.UtcNow;
}
