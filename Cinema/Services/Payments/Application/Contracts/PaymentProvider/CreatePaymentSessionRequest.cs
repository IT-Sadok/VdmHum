namespace Application.Contracts.PaymentProvider;

using Domain.Enums;

public sealed record CreatePaymentSessionRequest(
    Guid BookingId,
    decimal Amount,
    Currency Currency,
    string Description);