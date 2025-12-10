namespace Application.Commands.RequestPaymentRefund;

using Contracts.Payments;
using Domain.Enums;
using Shared.Contracts.Abstractions;

public sealed record RequestPaymentRefundCommand(
    Guid PaymentId,
    decimal Amount,
    Currency Currency,
    string? Reason,
    Guid? BookingRefundId
) : ICommand<PaymentRefundResponseModel>;