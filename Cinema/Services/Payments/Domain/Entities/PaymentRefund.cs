namespace Domain.Entities;

using Enums;
using ValueObjects;

public sealed class PaymentRefund
{
    private PaymentRefund()
    {
    }

    private PaymentRefund(
        Guid id,
        Guid paymentId,
        Money amount,
        string providerRefundId,
        DateTime requestedAtUtc,
        string? reason,
        Guid? bookingRefundId)
    {
        this.Id = id;
        this.PaymentId = paymentId;
        this.Amount = amount;
        this.ProviderRefundId = providerRefundId;
        this.RequestedAtUtc = requestedAtUtc;
        this.Reason = reason;
        this.BookingRefundId = bookingRefundId;
        this.Status = RefundStatus.Requested;
    }

    public Guid Id { get; private set; }

    public Guid PaymentId { get; private set; }

    public Guid? BookingRefundId { get; private set; }

    public Money Amount { get; private set; } = null!;

    public RefundStatus Status { get; private set; }

    public string? Reason { get; private set; }

    public string ProviderRefundId { get; private set; } = null!;

    public string? FailureCode { get; private set; }

    public string? FailureMessage { get; private set; }

    public DateTime RequestedAtUtc { get; private set; }

    public DateTime? SucceededAtUtc { get; private set; }

    public DateTime? FailedAtUtc { get; private set; }

    public static PaymentRefund Create(
        Guid paymentId,
        Money amount,
        string providerRefundId,
        DateTime requestedAtUtc,
        string? reason = null,
        Guid? bookingRefundId = null)
    {
        if (paymentId == Guid.Empty)
        {
            throw new ArgumentException("PaymentId cannot be empty.", nameof(paymentId));
        }

        ArgumentNullException.ThrowIfNull(amount);

        if (amount.Amount <= 0)
        {
            throw new ArgumentException("Refund amount must be greater than zero.", nameof(amount));
        }

        if (string.IsNullOrWhiteSpace(providerRefundId))
        {
            throw new ArgumentException("ProviderRefundId is required.", nameof(providerRefundId));
        }

        if (requestedAtUtc == default)
        {
            requestedAtUtc = DateTime.UtcNow;
        }

        return new PaymentRefund(
            id: Guid.CreateVersion7(),
            paymentId: paymentId,
            amount: amount,
            providerRefundId: providerRefundId,
            requestedAtUtc: requestedAtUtc,
            reason: reason,
            bookingRefundId: bookingRefundId);
    }

    public void MarkSucceeded(DateTime? succeededAtUtc = null)
    {
        if (this.Status == RefundStatus.Succeeded)
        {
            return;
        }

        if (this.Status == RefundStatus.Failed)
        {
            throw new InvalidOperationException("Cannot mark failed refund as succeeded.");
        }

        var timestamp = succeededAtUtc ?? DateTime.UtcNow;

        if (timestamp < this.RequestedAtUtc)
        {
            throw new ArgumentException("SucceededAtUtc cannot be earlier than RequestedAtUtc.", nameof(succeededAtUtc));
        }

        this.Status = RefundStatus.Succeeded;
        this.SucceededAtUtc = timestamp;
        this.FailureCode = null;
        this.FailureMessage = null;
    }

    public void MarkFailed(
        string failureCode,
        string failureMessage,
        DateTime? failedAtUtc = null)
    {
        if (this.Status == RefundStatus.Failed)
        {
            return;
        }

        if (this.Status == RefundStatus.Succeeded)
        {
            throw new InvalidOperationException("Cannot mark succeeded refund as failed.");
        }

        if (string.IsNullOrWhiteSpace(failureCode))
        {
            throw new ArgumentException("Failure code is required.", nameof(failureCode));
        }

        if (string.IsNullOrWhiteSpace(failureMessage))
        {
            throw new ArgumentException("Failure message is required.", nameof(failureMessage));
        }

        var timestamp = failedAtUtc ?? DateTime.UtcNow;

        if (timestamp < this.RequestedAtUtc)
        {
            throw new ArgumentException("FailedAtUtc cannot be earlier than RequestedAtUtc.", nameof(failedAtUtc));
        }

        this.Status = RefundStatus.Failed;
        this.FailedAtUtc = timestamp;
        this.FailureCode = failureCode;
        this.FailureMessage = failureMessage;
    }
}