namespace Application.Commands.CancelPendingBooking;

using Abstractions.Messaging;
using Contracts.Bookings;

public sealed record CancelPendingBookingCommand(
    Guid BookingId
) : ICommand<BookingResponseModel>;