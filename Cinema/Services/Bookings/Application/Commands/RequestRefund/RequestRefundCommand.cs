namespace Application.Commands.RequestRefund;

using Abstractions;
using Contracts.Bookings;

public sealed record RequestRefundCommand(Guid BookingId)
    : ICommand<BookingResponseModel>;