namespace Application.Abstractions.Services;

using Contracts.Payments;
using Domain.Enums;
using Shared.Contracts.Core;

public interface IPaymentsClient
{
    Task<Result<CreatePaymentForBookingResponse>> CreatePaymentForBookingAsync(
        Guid userId,
        Guid bookingId,
        decimal amount,
        Currency currency,
        string description,
        CancellationToken ct);

    Task<Result<CancelPaymentResponse>> CancelPaymentAsync(Guid paymentId, CancellationToken ct);
}