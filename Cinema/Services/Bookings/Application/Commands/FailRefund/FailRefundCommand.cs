namespace Application.Commands.FailRefund;

using Abstractions;

public sealed record FailRefundCommand(
    Guid BookingId,
    Guid RefundId,
    string FailureReason
) : ICommand<Guid>;