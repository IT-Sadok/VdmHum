namespace Application.Commands.ProcessBookingPayment;

using Abstractions;
using Contracts.Bookings;

public sealed record ProcessBookingPaymentCommand(
    Guid BookingId,
    string PaymentId,
    DateTime PaymentTime
) : ICommand<BookingResponseModel>;