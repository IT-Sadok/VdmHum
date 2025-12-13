namespace Application.Commands.ProcessBookingPayment;

using Contracts.Bookings;
using Shared.Contracts.Abstractions;

public sealed record ProcessBookingPaymentCommand(
    Guid BookingId,
    Guid PaymentId,
    DateTime PaymentTime
) : ICommand<BookingResponseModel>;