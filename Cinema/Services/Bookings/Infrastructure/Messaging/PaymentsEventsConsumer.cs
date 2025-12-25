namespace Infrastructure.Messaging;

using System.Text;
using System.Text.Json;
using Application.Commands.SystemCancelBooking;
using Azure.Messaging.ServiceBus;
using Domain.Enums;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Events;

public sealed class PaymentsEventsConsumer : BackgroundService
{
    private readonly ServiceBusProcessor _processor;
    private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly EventJsonOptions _jsonOptions;

    public PaymentsEventsConsumer(
        IConfiguration config,
        IServiceScopeFactory serviceScopeFactory,
        EventJsonOptions jsonOptions)
    {
        var client = new ServiceBusClient(config.GetConnectionString("ServiceBus"));
        this._processor = client.CreateProcessor("payments-events", new ServiceBusProcessorOptions());
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