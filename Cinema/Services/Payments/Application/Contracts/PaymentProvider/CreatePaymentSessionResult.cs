namespace Application.Contracts.PaymentProvider;

public sealed record CreatePaymentSessionResult(
    string ProviderPaymentId,
    string CheckoutUrl);