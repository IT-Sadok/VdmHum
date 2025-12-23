namespace Application.Abstractions.Services;

using Shared.Contracts.Abstractions;

public interface IEventPublisher
{
    Task PublishAsync(IEvent @event, CancellationToken ct);
}