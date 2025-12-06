namespace Application.Commands.SystemCancelBooking;

using Domain.Enums;
using Shared.Contracts.Abstractions;

public sealed record SystemCancelBookingCommand(
    Guid BookingId,
    BookingCancellationReason Reason
) : ICommand<Guid>;