using Telecube.BuildingBlocks.Common;

namespace Customer.Domain
{
    public class CustomerUpdatedDomainEvent(Guid id, string email, string displayName) : IDomainEvent
    {
        public Guid Id { get; } = id;
        public string Email { get; } = email;
        public string DisplayName { get; } = displayName;
        DateTimeOffset IDomainEvent.OccurredOn => DateTime.UtcNow;
    }
}