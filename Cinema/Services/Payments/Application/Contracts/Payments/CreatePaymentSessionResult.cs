namespace Application.Contracts.Payments;

public sealed record CreatePaymentSessionResult(
    string ProviderPaymentId,
    string CheckoutUrl);