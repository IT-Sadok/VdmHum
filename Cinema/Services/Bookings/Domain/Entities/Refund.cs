namespace Domain.Entities;

using Enums;
using ValueObjects;

public sealed class Refund
{
    private Refund(
        Guid id,
        Guid bookingId,
        Money amount,
        string reason,
        string? paymentId,
        DateTime requestedAtUtc)
    {
        this.Id = id;
        this.BookingId = bookingId;
        this.Amount = amount;
        this.Reason = reason;
        this.PaymentId = paymentId;
        this.RequestedAtUtc = requestedAtUtc;
        this.Status = RefundStatus.Requested;
    }

    public Guid Id { get; private set; }

    public Guid BookingId { get; private set; }

    public Money Amount { get; private set; }

    public string Reason { get; private set; }

    public RefundStatus Status { get; private set; }

    public DateTime RequestedAtUtc { get; private set; }

    public DateTime? ProcessedAtUtc { get; private set; }

    public string? PaymentId { get; private set; }

    public string? FailureReason { get; private set; }

    public static Refund Create(
        Guid bookingId,
        Money amount,
        string reason,
        string? paymentId)
    {
        if (bookingId == Guid.Empty)
        {
            throw new ArgumentException("BookingId cannot be empty.", nameof(bookingId));
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException("Refund reason is required.", nameof(reason));
        }

        var now = DateTime.UtcNow;

        return new Refund(
            id: Guid.NewGuid(),
            bookingId: bookingId,
            amount: amount,
            reason: reason,
            paymentId: paymentId,
            requestedAtUtc: now);
    }

    public void MarkInProgress()
    {
        if (this.Status != RefundStatus.Requested)
        {
            throw new InvalidOperationException(
                $"Only refunds in {RefundStatus.Requested} state can be moved to InProgress.");
        }

        this.Status = RefundStatus.InProgress;
    }

    public void MarkSucceeded(DateTime? processedAtUtc = null)
    {
        if (this.Status == RefundStatus.Succeeded)
        {
            return;
        }

        if (this.Status == RefundStatus.Failed)
        {
            throw new InvalidOperationException("Cannot mark failed refund as succeeded.");
        }

        this.Status = RefundStatus.Succeeded;
        this.ProcessedAtUtc = processedAtUtc ?? DateTime.UtcNow;
        this.FailureReason = null;
    }

    public void MarkFailed(string failureReason, DateTime? processedAtUtc = null)
    {
        if (this.Status == RefundStatus.Succeeded)
        {
            throw new InvalidOperationException("Cannot mark succeeded refund as failed.");
        }

        if (string.IsNullOrWhiteSpace(failureReason))
        {
            throw new ArgumentException("Failure reason is required.", nameof(failureReason));
        }

        this.Status = RefundStatus.Failed;
        this.ProcessedAtUtc = processedAtUtc ?? DateTime.UtcNow;
        this.FailureReason = failureReason;
    }
}