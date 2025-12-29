namespace Infrastructure.Messaging;

using System.Text;
using System.Text.Json;
using Application.Commands.SystemCancelBooking;
using Application.Options;
using Azure.Messaging.ServiceBus;
using Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Events;

public sealed class PaymentsEventsConsumer : BackgroundService
{
    private readonly ServiceBusProcessor _processor;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly EventJsonOptions _jsonOptions;

    public PaymentsEventsConsumer(
        ServiceBusClient client,
        IOptions<ServiceBusOptions> options,
        IServiceScopeFactory serviceScopeFactory,
        EventJsonOptions jsonOptions)
    {
        var sbOptions = options.Value;

        var topicName = sbOptions.Topics["Payments"];
        var subscriptionName = sbOptions.Subscriptions["BookingsService"];

        this._processor = client.CreateProcessor(topicName, subscriptionName);
        this._serviceScopeFactory = serviceScopeFactory;
        this._jsonOptions = jsonOptions;
    }

    protected override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        this._processor.ProcessMessageAsync += this.ProcessMessageAsync;
        this._processor.ProcessErrorAsync += args => Task.CompletedTask;

        return this._processor.StartProcessingAsync(stoppingToken);
    }

    private async Task ProcessMessageAsync(ProcessMessageEventArgs args)
    {
        if (args.Message.Subject == EventTypes.PaymentTransactionFailed)
        {
            var json = Encoding.UTF8.GetString(args.Message.Body);
            var @event = JsonSerializer.Deserialize<PaymentTransactionFailEvent>(json, this._jsonOptions.Options)!;

            using var scope = this._serviceScopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();

            var command = new SystemCancelBookingCommand(@event.BookingId, BookingCancellationReason.PaymentFailed);
            await mediator.ExecuteCommandAsync<SystemCancelBookingCommand, Guid>(command, args.CancellationToken);
        }

        await args.CompleteMessageAsync(args.Message);
    }
}