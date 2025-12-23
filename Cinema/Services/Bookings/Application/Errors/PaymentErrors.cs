namespace Application.Errors;

using Shared.Contracts.Core;

public static class PaymentErrors
{
    public static Error Unexpected(Exception e) => Error.Failure(
        code: "Payment.Unexpected",
        description: e.Message);
}