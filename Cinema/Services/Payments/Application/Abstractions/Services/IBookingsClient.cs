namespace Application.Abstractions.Services;

public interface IBookingsClient
{
    Task ProcessBookingPaymentAsync(
        Guid bookingId,
        Guid paymentId,
        DateTime paymentTime,
        CancellationToken ct);
}