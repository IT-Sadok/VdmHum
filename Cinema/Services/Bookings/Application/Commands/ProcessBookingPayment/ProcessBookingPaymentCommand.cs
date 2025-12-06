namespace Application.Commands.ProcessBookingPayment;

using Contracts.Bookings;
using Shared.Contracts.Abstractions;

public sealed record ProcessBookingPaymentCommand(
    Guid BookingId,
    string PaymentId,
    DateTime PaymentTime
) : ICommand<BookingResponseModel>;