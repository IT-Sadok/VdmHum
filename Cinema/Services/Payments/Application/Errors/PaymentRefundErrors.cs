namespace Application.Errors;

using Shared.Contracts.Core;

public static class PaymentRefundErrors
{
    public static readonly Error AlreadyProcessed = Error.Failure(
        code: "PaymentRefund.AlreadyProcessed",
        description: "Refund already processed.");
}