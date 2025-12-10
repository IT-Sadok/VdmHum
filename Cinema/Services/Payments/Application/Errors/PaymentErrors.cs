namespace Application.Errors;

using Shared.Contracts.Core;

public static class PaymentErrors
{
    public static readonly Error AlreadyProcessed = Error.Failure(
        code: "Payment.AlreadyProcessed",
        description: "Payment already processed.");
}