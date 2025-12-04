namespace Application.Commands.SystemCancelBooking;

using Abstractions.Messaging;
using Domain.Enums;

public sealed record SystemCancelBookingCommand(
    Guid BookingId,
    BookingCancellationReason Reason
) : ICommand<Guid>;