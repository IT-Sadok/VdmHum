namespace Application.Commands.CompleteRefund;

using Shared.Contracts.Abstractions;

public sealed record CompleteRefundCommand(
    Guid BookingId,
    Guid RefundId
) : ICommand<Guid>;