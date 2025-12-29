namespace Infrastructure.Messaging;

using System.Text;
using Application.Options;
using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Options;

public sealed class AzureServiceBusEventBus(
    ServiceBusClient client,
    IOptions<ServiceBusOptions> options)
    : IEventBus
{
    private readonly ServiceBusSender _sender = client.CreateSender(options.Value.PublisherTopic);

    public async Task PublishAsync(string eventType, string jsonContent, CancellationToken ct = default)
    {
        var body = Encoding.UTF8.GetBytes(jsonContent);
        var message = new ServiceBusMessage(body)
        {
            Subject = eventType,
        };

        await this._sender.SendMessageAsync(message, ct);
    }
}