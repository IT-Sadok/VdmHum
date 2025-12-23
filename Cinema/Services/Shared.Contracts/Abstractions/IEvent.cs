namespace Shared.Contracts.Abstractions;

public interface IEvent
{
    Guid Id { get; }
    DateTime OccurredOnUtc { get; }
}