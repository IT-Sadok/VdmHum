namespace Infrastructure.Messaging.Outbox;

using Application.Options;
using Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;

public sealed class OutboxProcessorBackgroundService(
    IEventBus eventBus,
    IServiceScopeFactory scopeFactory,
    IOptions<OutboxProcessorOptions> outboxProcessorOptions)
    : BackgroundService
{
    private readonly OutboxProcessorOptions _outboxOptions = outboxProcessorOptions.Value;

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        while (!ct.IsCancellationRequested)
        {
            using var scope = scopeFactory.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var messages = await dbContext.OutboxMessages
                .Where(m => m.ProcessedOnUtc == null)
                .OrderBy(m => m.OccurredOnUtc)
                .Take(this._outboxOptions.BatchSize)
                .ToListAsync(ct);

            foreach (var message in messages)
            {
                try
                {
                    await eventBus.PublishAsync(eventType: message.Type, jsonContent: message.Content, ct);

                    message.ProcessedOnUtc = DateTime.UtcNow;
                }
                catch (Exception ex)
                {
                    message.Error = ex.ToString();
                }
            }

            await dbContext.SaveChangesAsync(ct);

            await Task.Delay(TimeSpan.FromSeconds(this._outboxOptions.DelaySeconds), ct);
        }
    }
}