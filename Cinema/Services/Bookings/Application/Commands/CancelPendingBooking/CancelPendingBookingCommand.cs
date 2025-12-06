namespace Application.Commands.CancelPendingBooking;

using Contracts.Bookings;
using Shared.Contracts.Abstractions;

public sealed record CancelPendingBookingCommand(
    Guid BookingId
) : ICommand<BookingResponseModel>;