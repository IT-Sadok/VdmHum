namespace Application.Abstractions.Services;

using Contracts.Payments;
using Domain.Enums;

public interface IPaymentsGrpcClient
{
    Task<CreatePaymentForBookingResponse> CreatePaymentForBookingAsync(
        Guid bookingId,
        decimal amount,
        Currency currency,
        string description,
        CancellationToken ct);

    Task<CancelPaymentResponse> CancelPaymentAsync(Guid paymentId, CancellationToken ct);
}