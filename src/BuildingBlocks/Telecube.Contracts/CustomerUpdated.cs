namespace Telecube.Contracts;

public sealed record CustomerUpdated(Guid CustomerId, string Email, string DisplayName) : IIntegrationEvent
{
    public Guid EventId { get; init; } = Guid.NewGuid();
    public DateTimeOffset OccurredOn { get; init; } = DateTimeOffset.UtcNow;
}
