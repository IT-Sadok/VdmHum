namespace Application.Commands.RequestPaymentRefund;

using Contracts.Payments;
using Domain.Enums;
using Shared.Contracts.Abstractions;

public sealed record RequestPaymentRefundCommand(
    Guid PaymentId,
    decimal Amount,
    Currency Currency,
    Guid BookingRefundId,
    string? Reason
) : ICommand<PaymentRefundResponseModel>;