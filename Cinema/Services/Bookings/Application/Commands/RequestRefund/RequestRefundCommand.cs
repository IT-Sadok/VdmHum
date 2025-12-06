namespace Application.Commands.RequestRefund;

using Abstractions;
using Contracts.Bookings;
using Shared.Contracts.Abstractions;

public sealed record RequestRefundCommand(Guid BookingId)
    : ICommand<BookingResponseModel>, IAuthenticationRequired;