namespace Infrastructure.Messaging;

using Shared.Contracts.Abstractions;

public interface IEventBus
{
    Task PublishAsync(string eventType, string jsonContent, CancellationToken ct = default);
}