namespace Application.Errors;

using Shared.Contracts.Core;

public static class PaymentErrors
{
    public static Error Unexpected(Exception e) => Error.Failure(
        code: "Payment.Unexpected",
        description: e.Message);

    public static readonly Error CreationFailed = Error.Failure(
        code: "Payment.CreationFailed",
        description: "Payment creation failed.");
}