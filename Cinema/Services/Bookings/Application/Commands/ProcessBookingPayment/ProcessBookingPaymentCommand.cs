namespace Application.Commands.ProcessBookingPayment;

using Abstractions.Messaging;
using Contracts.Bookings;

public sealed record ProcessBookingPaymentCommand(
    Guid BookingId,
    string PaymentId,
    DateTime PaymentTime
) : ICommand<BookingResponseModel>;