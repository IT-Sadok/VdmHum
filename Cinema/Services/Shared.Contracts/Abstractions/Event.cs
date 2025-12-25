namespace Shared.Contracts.Abstractions;

public abstract record Event(string EventType) : IEvent
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public DateTime OccurredOnUtc { get; init; } = DateTime.UtcNow;
}