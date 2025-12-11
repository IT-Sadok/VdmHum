namespace Application.Contracts.PaymentProvider;

public sealed record CancelPaymentSessionRequest(string ProviderPaymentId);