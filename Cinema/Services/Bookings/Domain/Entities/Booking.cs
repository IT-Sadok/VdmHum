namespace Domain.Entities;

using Enums;
using ValueObjects;

public sealed class Booking
{
    private readonly List<BookingSeat> _seats = null!;

    private readonly List<Ticket> _tickets = null!;

    private readonly List<BookingRefund> _refunds = null!;

    private Booking()
    {
    }

    private Booking(
        Guid id,
        Guid userId,
        ShowtimeSnapshot showtime,
        List<BookingSeat> seats,
        Money totalPrice,
        DateTime createdAtUtc,
        DateTime reservationExpiresAtUtc)
    {
        this.Id = id;
        this.UserId = userId;
        this.Showtime = showtime;
        this._seats = seats;
        this.TotalPrice = totalPrice;
        this.CreatedAtUtc = createdAtUtc;
        this.ReservationExpiresAtUtc = reservationExpiresAtUtc;
        this.Status = BookingStatus.PendingPayment;
        this._tickets = [];
        this._refunds = [];
    }

    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public ShowtimeSnapshot Showtime { get; private set; } = null!;

    public BookingStatus Status { get; private set; }

    public Money TotalPrice { get; private set; } = null!;

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime? UpdatedAtUtc { get; private set; }

    public DateTime ReservationExpiresAtUtc { get; private set; }

    public Guid? PaymentId { get; private set; }

    public BookingCancellationReason? CancellationReason { get; private set; }

    public IReadOnlyCollection<BookingSeat> Seats => this._seats.AsReadOnly();

    public IReadOnlyCollection<Ticket> Tickets => this._tickets.AsReadOnly();

    public IReadOnlyCollection<BookingRefund> Refunds => this._refunds.AsReadOnly();

    public static Booking Create(
        Guid userId,
        ShowtimeSnapshot showtime,
        IEnumerable<int> seats,
        Money totalPrice,
        DateTime reservationExpiresAtUtc)
    {
        if (userId == Guid.Empty)
        {
            throw new ArgumentException("UserId cannot be empty.", nameof(userId));
        }

        ArgumentNullException.ThrowIfNull(showtime);

        ArgumentNullException.ThrowIfNull(seats);

        ArgumentNullException.ThrowIfNull(totalPrice);

        var seatsSet = new HashSet<int>(seats);

        if (seatsSet.Count == 0)
        {
            throw new ArgumentException("At least one seat must be selected.", nameof(seats));
        }

        var now = DateTime.UtcNow;

        if (reservationExpiresAtUtc <= now)
        {
            throw new ArgumentException(
                "Reservation expiration must be in the future.",
                nameof(reservationExpiresAtUtc));
        }

        var bookingId = Guid.CreateVersion7();

        var bookingSeats = seatsSet
            .Select(seatNumber => BookingSeat.Create(
                bookingId: bookingId,
                showtimeId: showtime.ShowtimeId,
                seatNumber: seatNumber))
            .ToList();

        var booking = new Booking(
            id: bookingId,
            userId: userId,
            showtime: showtime,
            seats: bookingSeats,
            totalPrice: totalPrice,
            createdAtUtc: now,
            reservationExpiresAtUtc: reservationExpiresAtUtc);

        return booking;
    }

    public void SetPayment(Guid paymentId)
    {
        if (paymentId == Guid.Empty)
        {
            throw new ArgumentException("PaymentId cannot be empty.", nameof(paymentId));
        }

        this.PaymentId = paymentId;
    }

    public void ConfirmPayment(Guid paymentId, DateTime paidAtUtc)
    {
        if (paymentId == Guid.Empty)
        {
            throw new ArgumentException("PaymentId cannot be empty.", nameof(paymentId));
        }

        // Idempotency: if this booking is already confirmed with the same paymentId,
        // we simply return without doing anything.
        if (this.Status == BookingStatus.Confirmed && this.PaymentId == paymentId)
        {
            return;
        }

        // If the booking is already confirmed with a different paymentId,
        // this indicates a data inconsistency or fraud.
        if (this.Status == BookingStatus.Confirmed && this.PaymentId != paymentId)
        {
            throw new InvalidOperationException("Booking is already confirmed with a different payment.");
        }

        // Only bookings waiting for payment can be confirmed.
        if (this.Status != BookingStatus.PendingPayment)
        {
            throw new InvalidOperationException(
                $"Cannot confirm payment when booking status is {this.Status}.");
        }

        // Business rule: payment must be completed within the reservation window.
        // If provider reports paidAtUtc after ReservationExpiresAtUtc, this is a late payment.
        // We don't handle the late-payment case here; the caller should call HandleLatePayment instead.
        if (paidAtUtc > this.ReservationExpiresAtUtc)
        {
            throw new InvalidOperationException("Payment was completed after reservation expired.");
        }

        // If a PaymentId is already set and it's different from the given one,
        // we again detect conflicting payment information.
        if (this.PaymentId is not null && this.PaymentId != paymentId)
        {
            throw new InvalidOperationException("Booking already has a different payment associated.");
        }

        // Set PaymentId if it was not set before (for first-time confirmation).
        this.PaymentId ??= paymentId;

        // Issue tickets for all reserved seats.
        foreach (var seatNumber in this._seats.Select(s => s.SeatNumber))
        {
            var ticketNumber = $"{this.Id:N}-{seatNumber}";
            var ticket = Ticket.Create(
                bookingId: this.Id,
                seatNumber: seatNumber,
                ticketNumber: ticketNumber,
                qrCode: null);

            this._tickets.Add(ticket);
        }

        // Once tickets are created, booking is considered fully confirmed.
        this.Status = BookingStatus.Confirmed;
        this.UpdatedAtUtc = DateTime.UtcNow;
    }

    public BookingRefund ProcessLatePayment()
    {
        // If booking is already confirmed or in any refund process,
        // receiving a "late" payment event doesn't make sense and should be treated as an error.
        if (this.Status is BookingStatus.Confirmed or BookingStatus.RefundPending or BookingStatus.Refunded)
        {
            throw new InvalidOperationException(
                $"Late payment received but booking is already {this.Status}.");
        }

        // We do not allow multiple active refunds for the same booking.
        var activeRefundExists = this._refunds.Any(r =>
            r.Status is RefundStatus.Requested);

        if (activeRefundExists)
        {
            throw new InvalidOperationException("There is already an active refund for this booking.");
        }

        var refund = BookingRefund.Create(
            bookingId: this.Id,
            amount: this.TotalPrice);

        this._refunds.Add(refund);

        this.Status = BookingStatus.RefundPending;
        this.CancellationReason = BookingCancellationReason.PaymentExpired;
        this.UpdatedAtUtc = DateTime.UtcNow;
        return refund;
    }

    public void CancelPendingPayment()
    {
        // We only allow direct cancellation while the booking is still waiting for payment.
        // Cancel after confirmation is handled via the refund flow, not this method.
        if (this.Status != BookingStatus.PendingPayment)
        {
            throw new InvalidOperationException(
                $"Only bookings in {BookingStatus.PendingPayment} status can be cancelled directly.");
        }

        this.Status = BookingStatus.Cancelled;
        this.CancellationReason = BookingCancellationReason.PaymentCanceled;
        this.UpdatedAtUtc = DateTime.UtcNow;
    }

    public void ExpireReservation()
    {
        // Expiration is meaningful only for bookings that are still pending payment.
        // If status is already something else, we silently do nothing (idempotent behavior).
        if (this.Status != BookingStatus.PendingPayment)
        {
            return;
        }

        this.Status = BookingStatus.Expired;
        this.CancellationReason ??= BookingCancellationReason.PaymentExpired;
        this.UpdatedAtUtc = DateTime.UtcNow;
    }

    public BookingRefund? CancelBySystem(BookingCancellationReason reason)
    {
        // If booking is already fully closed, just return (idempotent behavior).
        if (this.Status is BookingStatus.Cancelled
            or BookingStatus.Expired
            or BookingStatus.Refunded)
        {
            return null;
        }

        // CASE 1: Booking not paid yet – simple cancellation, no refund.
        if (this.Status == BookingStatus.PendingPayment)
        {
            this.Status = BookingStatus.Cancelled;
            this.CancellationReason = reason;
            this.UpdatedAtUtc = DateTime.UtcNow;

            // No refund created because nothing was successfully charged.
            return null;
        }

        // CASE 2: Booking was paid and confirmed – tickets exist and must be invalidated.
        if (this.Status == BookingStatus.Confirmed)
        {
            // Invalidate all tickets for safety: user must not be able to use them.
            foreach (var ticket in this._tickets)
            {
                ticket.Cancel();
            }

            this.CancellationReason = reason;

            // This reuses common refund rules (currency/amount checks, active refund, etc.)
            var createdRefund = this.RequestRefund();

            return createdRefund;
        }

        // Any other state – treat as unsupported for system cancel for now.
        throw new InvalidOperationException(
            $"System cancellation is not supported when booking status is {this.Status}.");
    }

    public BookingRefund RequestRefund()
    {
        // Refunds are only allowed for bookings that were successfully confirmed (tickets issued).
        if (this.Status != BookingStatus.Confirmed)
        {
            throw new InvalidOperationException(
                $"Refund can only be requested when booking is {BookingStatus.Confirmed}.");
        }

        // We do not allow multiple active refunds for the same booking.
        var activeRefundExists = this._refunds.Any(r =>
            r.Status is RefundStatus.Requested);

        if (activeRefundExists)
        {
            throw new InvalidOperationException("There is already an active refund for this booking.");
        }

        var refund = BookingRefund.Create(
            bookingId: this.Id,
            amount: this.TotalPrice);

        this._refunds.Add(refund);

        // All tickets are marked as cancelled so that they cannot be used for entry.
        foreach (var ticket in this._tickets)
        {
            ticket.Cancel();
        }

        this.Status = BookingStatus.RefundPending;
        this.CancellationReason = BookingCancellationReason.UserRefunded;
        this.UpdatedAtUtc = DateTime.UtcNow;

        return refund;
    }

    public void CompleteRefund(Guid refundId)
    {
        // We can only complete a refund when the booking is in RefundPending state.
        if (this.Status != BookingStatus.RefundPending)
        {
            throw new InvalidOperationException(
                $"Refund can only be completed when booking is {BookingStatus.RefundPending}.");
        }

        var refund = this._refunds.SingleOrDefault(r => r.Id == refundId);

        if (refund is null)
        {
            throw new InvalidOperationException("Refund not found for this booking.");
        }

        // Only refunds that are still in progress/requested can transition to Succeeded.
        if (refund.Status is not RefundStatus.Requested)
        {
            throw new InvalidOperationException(
                $"Refund must be in {RefundStatus.Requested} state to be completed.");
        }

        var now = DateTime.UtcNow;

        refund.MarkSucceeded(now);

        // Once refund is successful, booking moves to Refunded state.
        this.Status = BookingStatus.Refunded;
        this.UpdatedAtUtc = now;

        // All tickets are marked as refunded so that they cannot be used for entry.
        foreach (var ticket in this._tickets)
        {
            ticket.MarkRefunded(now);
        }
    }

    public void FailRefund(Guid refundId, string failureReason)
    {
        if (this.Status != BookingStatus.RefundPending)
        {
            throw new InvalidOperationException(
                $"Refund can only be marked as failed when booking is {BookingStatus.RefundPending}.");
        }

        if (string.IsNullOrWhiteSpace(failureReason))
        {
            throw new ArgumentException("Failure reason is required.", nameof(failureReason));
        }

        var refund = this._refunds.SingleOrDefault(r => r.Id == refundId);

        if (refund is null)
        {
            throw new InvalidOperationException("Refund not found for this booking.");
        }

        if (refund.Status is not RefundStatus.Requested)
        {
            throw new InvalidOperationException(
                $"Refund must be in {RefundStatus.Requested} state to be marked as failed.");
        }

        var now = DateTime.UtcNow;

        refund.MarkFailed(failureReason, now);

        this.Status = BookingStatus.RefundPending;
        this.UpdatedAtUtc = now;
    }
}