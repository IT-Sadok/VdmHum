namespace Application.Commands.FailRefund;

using Abstractions.Messaging;

public sealed record FailRefundCommand(
    Guid BookingId,
    Guid RefundId,
    string FailureReason
) : ICommand<Guid>;