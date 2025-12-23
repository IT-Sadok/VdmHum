namespace Infrastructure.Messaging.Outbox;

using System.Text.Json;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Contracts.Abstractions;

public sealed class OutboxProcessorBackgroundService(
    IEventBus eventBus,
    IServiceScopeFactory scopeFactory,
    EventJsonOptions jsonOptions)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var messages = await dbContext.OutboxMessages
                .Where(m => m.ProcessedOnUtc == null)
                .OrderBy(m => m.OccurredOnUtc)
                .Take(100)
                .ToListAsync(ct);

            foreach (var message in messages)
            {
                try
                {
                    var type = Type.GetType(message.Type)!;
                    var @event = (IEvent)JsonSerializer.Deserialize(message.Content, type, jsonOptions.Options)!;

                    await eventBus.PublishAsync(@event, ct);

                    message.ProcessedOnUtc = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    message.Error = ex.ToString();
                }
            }

            await dbContext.SaveChangesAsync(ct);

            // TODO: Move magic number to some config
            await Task.Delay(TimeSpan.FromSeconds(1), ct);
        }
    }
}