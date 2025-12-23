namespace Infrastructure.Messaging;

using System.Text.Json;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Shared.Contracts.Abstractions;

public sealed class AzureServiceBusEventBus : IEventBus
{
    private readonly ServiceBusSender _sender;

    public AzureServiceBusEventBus(IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("ServiceBus");
        var client = new ServiceBusClient(connectionString);
        this._sender = client.CreateSender("bookings-events");
    }

    public async Task PublishAsync(IEvent @event, CancellationToken ct = default)
    {
        var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType());
        var message = new ServiceBusMessage(body)
        {
            Subject = @event.GetType().Name,
        };

        await this._sender.SendMessageAsync(message, ct);
    }
}