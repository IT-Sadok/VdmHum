namespace Application.Commands.CompleteRefund;

using Abstractions;

public sealed record CompleteRefundCommand(
    Guid BookingId,
    Guid RefundId
) : ICommand<Guid>;