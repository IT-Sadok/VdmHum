namespace Application.Commands.CancelPendingBooking;

using Abstractions;
using Contracts.Bookings;

public sealed record CancelPendingBookingCommand(
    Guid BookingId
) : ICommand<BookingResponseModel>;