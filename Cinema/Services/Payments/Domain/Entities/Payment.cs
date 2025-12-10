namespace Domain.Entities;

using Enums;
using ValueObjects;

public sealed class Payment
{
    private readonly List<PaymentRefund> _refunds = [];

    private Payment()
    {
    }

    private Payment(
        Guid id,
        Guid bookingId,
        Money amount,
        PaymentProvider provider,
        string providerPaymentId,
        string? checkoutUrl,
        DateTime createdAtUtc)
    {
        this.Id = id;
        this.BookingId = bookingId;
        this.Amount = amount;
        this.Provider = provider;
        this.ProviderPaymentId = providerPaymentId;
        this.CheckoutUrl = checkoutUrl;
        this.CreatedAtUtc = createdAtUtc;

        this.Status = PaymentStatus.Pending;
    }

    public Guid Id { get; private set; }

    public Guid BookingId { get; private set; }

    public Money Amount { get; private set; } = null!;

    public PaymentStatus Status { get; private set; }

    public PaymentProvider Provider { get; private set; }

    public string ProviderPaymentId { get; private set; } = null!;

    public string? CheckoutUrl { get; private set; }

    public string? FailureCode { get; private set; }

    public string? FailureMessage { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime? UpdatedAtUtc { get; private set; }

    public DateTime? SucceededAtUtc { get; private set; }

    public DateTime? FailedAtUtc { get; private set; }

    public DateTime? CanceledAtUtc { get; private set; }

    public IReadOnlyCollection<PaymentRefund> Refunds => this._refunds.AsReadOnly();

    public static Payment Create(
        Guid bookingId,
        Money amount,
        PaymentProvider provider,
        string providerPaymentId,
        string? checkoutUrl)
    {
        if (bookingId == Guid.Empty)
        {
            throw new ArgumentException("BookingId cannot be empty.", nameof(bookingId));
        }

        ArgumentNullException.ThrowIfNull(amount);

        if (amount.Amount <= 0)
        {
            throw new ArgumentException("Payment amount must be greater than zero.", nameof(amount));
        }

        if (string.IsNullOrWhiteSpace(providerPaymentId))
        {
            throw new ArgumentException("ProviderPaymentId is required.", nameof(providerPaymentId));
        }

        var id = Guid.CreateVersion7();
        var now = DateTime.UtcNow;

        return new Payment(
            id: id,
            bookingId: bookingId,
            amount: amount,
            provider: provider,
            providerPaymentId: providerPaymentId,
            checkoutUrl: checkoutUrl,
            createdAtUtc: now);
    }

    public void MarkSucceeded(DateTime succeededAtUtc)
    {
        if (this.Status is PaymentStatus.Succeeded or PaymentStatus.PartiallyRefunded or PaymentStatus.Refunded)
        {
            return;
        }

        if (this.Status is PaymentStatus.Failed or PaymentStatus.Canceled)
        {
            throw new InvalidOperationException($"Cannot mark payment as succeeded when status is {this.Status}.");
        }

        if (succeededAtUtc < this.CreatedAtUtc)
        {
            throw new ArgumentException("SucceededAtUtc cannot be earlier than CreatedAtUtc.", nameof(succeededAtUtc));
        }

        this.SucceededAtUtc = succeededAtUtc;
        this.UpdatedAtUtc = DateTime.UtcNow;
    }

    public void MarkFailed(
        string failureCode,
        string failureMessage,
        DateTime failedAtUtc)
    {
        if (this.Status == PaymentStatus.Failed)
        {
            return;
        }

        if (this.Status is PaymentStatus.Succeeded or PaymentStatus.PartiallyRefunded or PaymentStatus.Refunded)
        {
            throw new InvalidOperationException($"Cannot mark payment as failed when status is {this.Status}.");
        }

        if (string.IsNullOrWhiteSpace(failureCode))
        {
            throw new ArgumentException("Failure code is required.", nameof(failureCode));
        }

        if (string.IsNullOrWhiteSpace(failureMessage))
        {
            throw new ArgumentException("Failure message is required.", nameof(failureMessage));
        }

        if (failedAtUtc < this.CreatedAtUtc)
        {
            throw new ArgumentException("FailedAtUtc cannot be earlier than CreatedAtUtc.", nameof(failedAtUtc));
        }

        this.Status = PaymentStatus.Failed;
        this.FailureCode = failureCode;
        this.FailureMessage = failureMessage;
        this.FailedAtUtc = failedAtUtc;
        this.UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Cancel(DateTime canceledAtUtc)
    {
        if (this.Status == PaymentStatus.Canceled)
        {
            return;
        }

        if (this.Status is PaymentStatus.Succeeded or PaymentStatus.PartiallyRefunded or PaymentStatus.Refunded)
        {
            throw new InvalidOperationException($"Cannot cancel payment when status is {this.Status}.");
        }

        if (canceledAtUtc < this.CreatedAtUtc)
        {
            throw new ArgumentException("CanceledAtUtc cannot be earlier than CreatedAtUtc.", nameof(canceledAtUtc));
        }

        this.Status = PaymentStatus.Canceled;
        this.CanceledAtUtc = canceledAtUtc;
        this.UpdatedAtUtc = DateTime.UtcNow;
    }

    public PaymentRefund RequestRefund(
        Money amount,
        Money remainingAmountToRefund,
        string providerRefundId,
        DateTime requestedAtUtc,
        string? reason = null,
        Guid? bookingRefundId = null)
    {
        if (this.Status is not (PaymentStatus.Succeeded or PaymentStatus.PartiallyRefunded))
        {
            throw new InvalidOperationException(
                $"Refund can only be requested when payment is {PaymentStatus.Succeeded} or {PaymentStatus.PartiallyRefunded}.");
        }

        ArgumentNullException.ThrowIfNull(amount);

        if (amount.Amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Refund amount must be greater than zero.");
        }

        if (amount.Amount > remainingAmountToRefund.Amount)
        {
            throw new InvalidOperationException("Refund amount cannot exceed remaining refundable amount.");
        }

        var refund = PaymentRefund.Create(
            paymentId: this.Id,
            amount: amount,
            providerRefundId: providerRefundId,
            requestedAtUtc: requestedAtUtc,
            reason: reason,
            bookingRefundId: bookingRefundId);

        this._refunds.Add(refund);
        this.UpdatedAtUtc = DateTime.UtcNow;

        return refund;
    }

    public void CompleteRefund(Guid refundId, Money remainingAmountToRefund, DateTime? succeededAtUtc = null)
    {
        var refund = this._refunds.SingleOrDefault(r => r.Id == refundId);

        if (refund is null)
        {
            throw new InvalidOperationException("Refund not found for this payment.");
        }

        if (refund.Status is not RefundStatus.Requested)
        {
            throw new InvalidOperationException(
                $"Refund must be in {RefundStatus.Requested} state to be completed.");
        }

        refund.MarkSucceeded(succeededAtUtc);

        this.UpdatedAtUtc = DateTime.UtcNow;

        this.Status = remainingAmountToRefund.Amount <= 0
            ? PaymentStatus.Refunded
            : PaymentStatus.PartiallyRefunded;
    }

    public void FailRefund(
        Guid refundId,
        string failureCode,
        string failureMessage,
        DateTime? failedAtUtc = null)
    {
        var refund = this._refunds.SingleOrDefault(r => r.Id == refundId);

        if (refund is null)
        {
            throw new InvalidOperationException("Refund not found for this payment.");
        }

        if (refund.Status is not RefundStatus.Requested)
        {
            throw new InvalidOperationException(
                $"Refund must be in {RefundStatus.Requested} state to be marked as failed.");
        }

        refund.MarkFailed(failureCode, failureMessage, failedAtUtc);

        this.UpdatedAtUtc = DateTime.UtcNow;
    }
}