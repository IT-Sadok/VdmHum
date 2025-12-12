namespace Application.Abstractions.Services;

public interface IBookingsGrpcClient
{
    Task ProcessBookingPaymentAsync(
        Guid bookingId,
        Guid paymentId,
        DateTime paymentTime,
        CancellationToken ct);
}