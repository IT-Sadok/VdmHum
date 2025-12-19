namespace Infrastructure.Services;

using Application.Abstractions.Services;
using Bookings.Grpc;
using Google.Protobuf.WellKnownTypes;

public sealed class BookingsGrpcClient(Bookings.BookingsClient client)
    : IBookingsClient
{
    public async Task ProcessBookingPaymentAsync(
        Guid bookingId,
        Guid paymentId,
        DateTime paymentTime,
        CancellationToken ct)
    {
        var request = new ProcessBookingPaymentRequest
        {
            BookingId = bookingId.ToString(),
            PaymentId = paymentId.ToString(),
            PaymentTime = Timestamp.FromDateTime(paymentTime.ToUniversalTime()),
        };

        await client.ProcessBookingPaymentAsync(request, cancellationToken: ct);
    }
}