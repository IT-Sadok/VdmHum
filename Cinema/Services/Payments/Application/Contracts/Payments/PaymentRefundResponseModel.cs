namespace Application.Contracts.Payments;

using Domain.Enums;

public record PaymentRefundResponseModel(
    Guid Id,
    Guid PaymentId,
    Guid? BookingRefundId,
    decimal Amount,
    Currency Currency,
    RefundStatus Status,
    string? Reason,
    string ProviderRefundId,
    string? FailureCode,
    string? FailureMessage,
    DateTime RequestedAtUtc,
    DateTime? SucceededAtUtc,
    DateTime? FailedAtUtc);