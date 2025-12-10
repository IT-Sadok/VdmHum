namespace Application.Contracts.Payments;

using Domain.Enums;

public record PaymentResponseModel(
    Guid Id,
    Guid BookingId,
    decimal Amount,
    Currency Currency,
    PaymentStatus Status,
    PaymentProvider Provider,
    string ProviderPaymentId,
    string? CheckoutUrl,
    string? FailureCode,
    string? FailureMessage,
    DateTime CreatedAtUtc,
    DateTime? UpdatedAtUtc,
    DateTime? SucceededAtUtc,
    DateTime? FailedAtUtc,
    DateTime? CanceledAtUtc,
    IReadOnlyCollection<PaymentRefundResponseModel> Refunds);