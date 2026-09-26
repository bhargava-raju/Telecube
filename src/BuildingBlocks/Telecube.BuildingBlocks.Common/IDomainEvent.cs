namespace Telecube.BuildingBlocks.Common;

public interface IDomainEvent
{
    DateTimeOffset OccurredOn { get; }
}