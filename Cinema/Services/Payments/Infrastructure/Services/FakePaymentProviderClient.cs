namespace Infrastructure.Services;

using System.Net;
using Application.Abstractions.Services;
using Application.Commands.HandleProviderPaymentFailed;
using Application.Commands.HandleProviderPaymentSucceeded;
using Application.Contracts.PaymentProvider;
using Application.Exceptions;
using Shared.Contracts.Abstractions;

public sealed class FakePaymentProviderClient(IMediator mediator) : IPaymentProviderClient
{
    private const double FailureProbability = 0.2;

    private static readonly Random Random = new();

    public Task<CreatePaymentSessionResult> CreatePaymentSessionAsync(
        CreatePaymentSessionRequest request,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        MaybeThrowRandomError("CreatePaymentSession");

        var providerPaymentId = $"pay_{Guid.NewGuid():N}";
        var checkoutUrl = $"https://fake-payments.local/checkout/{providerPaymentId}";

        var result = new CreatePaymentSessionResult(
            ProviderPaymentId: providerPaymentId,
            CheckoutUrl: checkoutUrl);

        _ = Task.Run(
            async () =>
            {
                // emulate small delay
                await Task.Delay(TimeSpan.FromSeconds(2), ct);

                var succeeded = Random.NextDouble() >= FailureProbability;

                if (succeeded)
                {
                    var succeededCommand = new HandleProviderPaymentSucceededCommand(
                        ProviderPaymentId: providerPaymentId,
                        SucceededAtUtc: DateTime.UtcNow);

                    await mediator.ExecuteCommandAsync(
                        succeededCommand,
                        CancellationToken.None);
                }
                else
                {
                    var failedCommand = new HandleProviderPaymentFailedCommand(
                        ProviderPaymentId: providerPaymentId,
                        FailureCode: "fake_error",
                        FailureMessage: "Simulated payment failure",
                        FailedAtUtc: DateTime.UtcNow);

                    await mediator.ExecuteCommandAsync(
                        failedCommand,
                        CancellationToken.None);
                }
            }, ct);

        return Task.FromResult(result);
    }

    public Task CancelPaymentSessionAsync(
        CancelPaymentSessionRequest request,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        MaybeThrowRandomError("CancelPaymentSession");

        return Task.CompletedTask;
    }

    public Task<string> CreateRefundAsync(
        CreateRefundRequest request,
        CancellationToken ct)
    {
        ct.ThrowIfCancellationRequested();

        MaybeThrowRandomError("CreateRefund");

        var providerRefundId = $"refund_{Guid.NewGuid():N}";

        return Task.FromResult(providerRefundId);
    }

    private static void MaybeThrowRandomError(string operation)
    {
        var value = Random.NextDouble();

        if (value >= FailureProbability)
        {
            return;
        }

        var clientError = Random.Next(0, 2) == 0;

        var statusCode = clientError
            ? HttpStatusCode.BadRequest
            : HttpStatusCode.InternalServerError;

        throw new PaymentProviderException(
            message: $"Simulated {operation} error: {(int)statusCode} {statusCode}",
            statusCode: statusCode);
    }
}