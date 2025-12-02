namespace Domain.Entities;

using Enums;
using ValueObjects;

public sealed class Booking
{
    private readonly List<int> _seats;

    private readonly List<Ticket> _tickets;

    private readonly List<Refund> _refunds;

    private Booking(
        Guid id,
        Guid userId,
        ShowtimeSnapshot showtime,
        IEnumerable<int> seats,
        Money totalPrice,
        DateTime createdAtUtc,
        DateTime reservationExpiresAtUtc)
    {
        this.Id = id;
        this.UserId = userId;
        this.Showtime = showtime;
        this._seats = seats.Distinct().ToList();
        this.TotalPrice = totalPrice;
        this.CreatedAtUtc = createdAtUtc;
        this.ReservationExpiresAtUtc = reservationExpiresAtUtc;
        this.Status = BookingStatus.PendingPayment;
        this._tickets = [];
        this._refunds = [];
    }

    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public ShowtimeSnapshot Showtime { get; private set; }

    public BookingStatus Status { get; private set; }

    public Money TotalPrice { get; private set; }

    public DateTime CreatedAtUtc { get; private set; }

    public DateTime? UpdatedAtUtc { get; private set; }

    public DateTime ReservationExpiresAtUtc { get; private set; }

    public string? PaymentId { get; private set; }

    public string? CancellationReason { get; private set; }

    public IReadOnlyCollection<int> Seats => this._seats.AsReadOnly();

    public IReadOnlyCollection<Ticket> Tickets => this._tickets.AsReadOnly();

    public IReadOnlyCollection<Refund> Refunds => this._refunds.AsReadOnly();

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

        var seatsList = seats.Distinct().ToList();

        if (seatsList.Count == 0)
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

        var booking = new Booking(
            id: Guid.NewGuid(),
            userId: userId,
            showtime: showtime,
            seats: seatsList,
            totalPrice: totalPrice,
            createdAtUtc: now,
            reservationExpiresAtUtc: reservationExpiresAtUtc);

        return booking;
    }

    public void MarkPaymentPending(string paymentId)
    {
        if (string.IsNullOrWhiteSpace(paymentId))
        {
            throw new ArgumentException("PaymentId is required.", nameof(paymentId));
        }

        if (this.Status != BookingStatus.PendingPayment)
        {
            throw new InvalidOperationException(
                $"Cannot mark payment pending when booking status is {this.Status}.");
        }

        if (this.PaymentId == paymentId)
        {
            return;
        }

        if (this.PaymentId is not null && this.PaymentId != paymentId)
        {
            throw new InvalidOperationException("Booking already has a different payment associated.");
        }

        this.PaymentId = paymentId;
        this.UpdatedAtUtc = DateTime.UtcNow;
    }

    public void ConfirmPayment()
    {
        if (this.Status != BookingStatus.PendingPayment)
        {
            throw new InvalidOperationException($"Cannot confirm payment when booking status is {this.Status}.");
        }

        var now = DateTime.UtcNow;

        if (now > this.ReservationExpiresAtUtc)
        {
            this.Expire();
            throw new InvalidOperationException("Reservation has already expired.");
        }

        if (this.PaymentId is null)
        {
            throw new InvalidOperationException("PaymentId must be set before confirming payment.");
        }

        foreach (var seat in this._seats)
        {
            if (this._tickets.Any(t => t.SeatNumber == seat))
            {
                continue;
            }

            var ticketNumber = $"{this.Id:N}-{seat}";
            var ticket = Ticket.Create(
                bookingId: this.Id,
                seatNumber: seat,
                ticketNumber: ticketNumber,
                qrCode: null);

            this._tickets.Add(ticket);
        }

        this.Status = BookingStatus.Confirmed;
        this.UpdatedAtUtc = now;
    }

    public void Cancel(string reason)
    {
        if (this.Status != BookingStatus.PendingPayment)
        {
            throw new InvalidOperationException(
                $"Only bookings in {BookingStatus.PendingPayment} status can be cancelled directly.");
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException("Cancellation reason is required.", nameof(reason));
        }

        this.Status = BookingStatus.Cancelled;
        this.CancellationReason = reason;
        this.UpdatedAtUtc = DateTime.UtcNow;
    }

    public void Expire()
    {
        if (this.Status != BookingStatus.PendingPayment)
        {
            return;
        }

        this.Status = BookingStatus.Expired;
        this.CancellationReason ??= "Reservation expired.";
        this.UpdatedAtUtc = DateTime.UtcNow;
    }

    public void RequestRefund(Money amount, string reason)
    {
        if (this.Status != BookingStatus.Confirmed)
        {
            throw new InvalidOperationException(
                $"Refund can only be requested when booking is {BookingStatus.Confirmed}.");
        }

        if (amount.Amount > this.TotalPrice.Amount)
        {
            throw new InvalidOperationException("Refund amount cannot exceed total booking price.");
        }

        if (!string.Equals(amount.Currency, this.TotalPrice.Currency, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException("Refund currency does not match booking currency.");
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException("Refund reason is required.", nameof(reason));
        }

        var activeRefundExists = this._refunds.Any(r =>
            r.Status is RefundStatus.Requested or RefundStatus.InProgress);

        if (activeRefundExists)
        {
            throw new InvalidOperationException("There is already an active refund for this booking.");
        }

        var refund = Refund.Create(
            bookingId: this.Id,
            amount: amount,
            reason: reason,
            paymentId: this.PaymentId);

        this._refunds.Add(refund);

        this.Status = BookingStatus.RefundPending;
        this.CancellationReason = reason;
        this.UpdatedAtUtc = DateTime.UtcNow;
    }

    public void MarkLastRefundSucceeded()
    {
        if (this.Status != BookingStatus.RefundPending)
        {
            throw new InvalidOperationException(
                $"Refund can only be completed when booking is {BookingStatus.RefundPending}.");
        }

        var now = DateTime.UtcNow;

        var refund = this._refunds
            .LastOrDefault(r => r.Status is RefundStatus.Requested or RefundStatus.InProgress);

        if (refund is null)
        {
            throw new InvalidOperationException("No active refund found for this booking.");
        }

        refund.MarkSucceeded(now);

        this.Status = BookingStatus.Refunded;
        this.UpdatedAtUtc = now;

        foreach (var ticket in this._tickets)
        {
            ticket.MarkRefunded(now);
        }
    }

    public void MarkLastRefundFailed(string failureReason)
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

        var refund = this._refunds
            .LastOrDefault(r => r.Status is RefundStatus.Requested or RefundStatus.InProgress);

        if (refund is null)
        {
            throw new InvalidOperationException("No active refund found for this booking.");
        }

        var now = DateTime.UtcNow;

        refund.MarkFailed(failureReason, now);

        this.Status = BookingStatus.Confirmed;
        this.UpdatedAtUtc = now;
    }
}