namespace Application.Contracts.Payments;

using Domain.Entities;
using Shared.Contracts.Core;

public static class PaymentMappings
{
    public static Result<PaymentResponseModel> ToResponse(
        this Payment payment,
        bool includeRefunds = false)
    {
        var refunds = includeRefunds
            ? payment.Refunds.Select(r => r.ToResponse()).ToArray()
            : [];

        var dto = new PaymentResponseModel(
            Id: payment.Id,
            BookingId: payment.BookingId,
            Amount: payment.Amount.Amount,
            Currency: payment.Amount.Currency,
            Status: payment.Status,
            Provider: payment.Provider,
            ProviderPaymentId: payment.ProviderPaymentId,
            CheckoutUrl: payment.CheckoutUrl,
            FailureCode: payment.FailureCode,
            FailureMessage: payment.FailureMessage,
            CreatedAtUtc: payment.CreatedAtUtc,
            UpdatedAtUtc: payment.UpdatedAtUtc,
            SucceededAtUtc: payment.SucceededAtUtc,
            FailedAtUtc: payment.FailedAtUtc,
            CanceledAtUtc: payment.CanceledAtUtc,
            Refunds: refunds);

        return Result.Success(dto);
    }

    public static PaymentRefundResponseModel ToResponse(this PaymentRefund refund)
    {
        return new PaymentRefundResponseModel(
            Id: refund.Id,
            PaymentId: refund.PaymentId,
            BookingRefundId: refund.BookingRefundId,
            Amount: refund.Amount.Amount,
            Currency: refund.Amount.Currency,
            Status: refund.Status,
            Reason: refund.Reason,
            ProviderRefundId: refund.ProviderRefundId,
            FailureCode: refund.FailureCode,
            FailureMessage: refund.FailureMessage,
            RequestedAtUtc: refund.RequestedAtUtc,
            SucceededAtUtc: refund.SucceededAtUtc,
            FailedAtUtc: refund.FailedAtUtc);
    }
}