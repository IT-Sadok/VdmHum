namespace Infrastructure.Services;

using System.Net;
using Application.Abstractions.Services;
using Application.Contracts.PaymentProvider;
using Application.Exceptions;

public sealed class FakePaymentProviderClient : IPaymentProviderClient
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