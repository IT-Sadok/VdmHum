namespace Infrastructure.Services;

using System.Net;
using Application.Abstractions.Services;
using Application.Contracts.PaymentProvider;
using Application.Exceptions;
using Polly;

public sealed class RetryingPaymentProviderClient(IPaymentProviderClient inner) : IPaymentProviderClient
{
    private readonly AsyncPolicy _retryPolicy = Policy
        .Handle<PaymentProviderException>(ex =>
            ex.StatusCode is HttpStatusCode.InternalServerError or HttpStatusCode.BadRequest)
        .WaitAndRetryAsync(
            retryCount: 3,
            sleepDurationProvider: attempt => TimeSpan.FromMilliseconds(100 * Math.Pow(2, attempt)));

    public Task<CreatePaymentSessionResult> CreatePaymentSessionAsync(
        CreatePaymentSessionRequest request,
        CancellationToken ct) =>
        this._retryPolicy.ExecuteAsync(
            cancellationToken => inner.CreatePaymentSessionAsync(request, cancellationToken),
            ct);

    public Task CancelPaymentSessionAsync(
        CancelPaymentSessionRequest request,
        CancellationToken ct) =>
        this._retryPolicy.ExecuteAsync(
            cancellationToken => inner.CancelPaymentSessionAsync(request, cancellationToken),
            ct);

    public Task<string> CreateRefundAsync(
        CreateRefundRequest request,
        CancellationToken ct) =>
        this._retryPolicy.ExecuteAsync(
            cancellationToken => inner.CreateRefundAsync(request, cancellationToken),
            ct);
}