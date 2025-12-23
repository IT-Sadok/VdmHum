namespace Infrastructure.Messaging.Outbox;

using System.Text.Json;
using Application.Abstractions.Services;
using Database;
using Shared.Contracts.Abstractions;

public sealed class OutboxEventPublisher(
    ApplicationDbContext dbContext,
    EventJsonOptions jsonOptions)
    : IEventPublisher
{
    public async Task PublishAsync(IEvent @event, CancellationToken ct)
    {
        var outboxMessage = new OutboxMessage
        {
            Id = @event.Id,
            OccurredOnUtc = @event.OccurredOnUtc,
            Type = @event.GetType().AssemblyQualifiedName!,
            Content = JsonSerializer.Serialize(@event, @event.GetType(), jsonOptions.Options),
        };

        await dbContext.OutboxMessages.AddAsync(outboxMessage, ct);
    }
}