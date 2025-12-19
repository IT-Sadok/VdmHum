namespace Application.Errors;

using Shared.Contracts.Core;

public static class PaymentProviderErrors
{
    public static readonly Error BadRequest = Error.Failure(
        code: "PaymentProvider.BadRequest",
        description: "Payment provider rejected the request.");

    public static readonly Error ServerError = Error.Failure(
        code: "PaymentProvider.ServerError",
        description: "Payment provider is currently unavailable or returned an error.");
}