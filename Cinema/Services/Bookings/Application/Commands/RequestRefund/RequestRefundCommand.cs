namespace Application.Commands.RequestRefund;

using Abstractions.Messaging;
using Contracts.Bookings;

public sealed record RequestRefundCommand(Guid BookingId)
    : ICommand<BookingResponseModel>;