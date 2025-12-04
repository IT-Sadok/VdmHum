namespace Application.Commands.CompleteRefund;

using Abstractions.Messaging;

public sealed record CompleteRefundCommand(
    Guid BookingId,
    Guid RefundId
) : ICommand<Guid>;