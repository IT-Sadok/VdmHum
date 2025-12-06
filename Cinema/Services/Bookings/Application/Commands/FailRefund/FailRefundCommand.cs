namespace Application.Commands.FailRefund;

using Shared.Contracts.Abstractions;

public sealed record FailRefundCommand(
    Guid BookingId,
    Guid RefundId,
    string FailureReason
) : ICommand<Guid>;