namespace Application.Contracts.PaymentProvider;

using Domain.Enums;

public sealed record CreateRefundRequest(
    string ProviderPaymentId,
    decimal Amount,
    Currency Currency,
    string? Reason);