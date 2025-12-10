namespace Application.Abstractions.Services;

using Contracts.PaymentProvider;

public interface IPaymentProviderClient
{
    Task<CreatePaymentSessionResult> CreatePaymentSessionAsync(
        CreatePaymentSessionRequest request,
        CancellationToken ct);

    Task CancelPaymentSessionAsync(
        CancelPaymentSessionRequest request,
        CancellationToken ct);

    Task<string> CreateRefundAsync(
        CreateRefundRequest request,
        CancellationToken ct);
}