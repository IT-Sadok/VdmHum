namespace Application.Abstractions.Services;

using Contracts.Payments;
using Domain.Enums;

public interface IPaymentProviderClient
{
    Task<CreatePaymentSessionResult> CreatePaymentSessionAsync(
        Guid bookingId,
        decimal amount,
        Currency currency,
        string description,
        CancellationToken ct);

    Task CancelPaymentSessionAsync(string providerPaymentId, CancellationToken ct);

    Task<string> CreateRefundAsync(
        string providerPaymentId,
        decimal amount,
        Currency currency,
        string? reason,
        CancellationToken ct);
}