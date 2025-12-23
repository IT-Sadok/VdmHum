namespace Infrastructure.Messaging;

using Shared.Contracts.Abstractions;

public interface IEventBus
{
    Task PublishAsync(IEvent @event, CancellationToken ct = default);
}