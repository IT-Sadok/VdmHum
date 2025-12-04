namespace Domain.Entities;

using Enums;

public sealed class Ticket
{
    private Ticket()
    {
    }

    private Ticket(
        Guid id,
        Guid bookingId,
        int seatNumber,
        string ticketNumber,
        string? qrCode,
        DateTime issuedAtUtc)
    {
        this.Id = id;
        this.BookingId = bookingId;
        this.SeatNumber = seatNumber;
        this.TicketNumber = ticketNumber;
        this.QrCode = qrCode;
        this.IssuedAtUtc = issuedAtUtc;
        this.Status = TicketStatus.Active;
    }

    public Guid Id { get; private set; }

    public Guid BookingId { get; private set; }

    public int SeatNumber { get; private set; }

    public string TicketNumber { get; private set; } = null!;

    public string? QrCode { get; private set; }

    public TicketStatus Status { get; private set; }

    public DateTime IssuedAtUtc { get; private set; }

    public DateTime? CancelledAtUtc { get; private set; }

    public static Ticket Create(
        Guid bookingId,
        int seatNumber,
        string ticketNumber,
        string? qrCode)
    {
        if (bookingId == Guid.Empty)
        {
            throw new ArgumentException("BookingId cannot be empty.", nameof(bookingId));
        }

        if (seatNumber <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(seatNumber), "Seat number must be positive.");
        }

        if (string.IsNullOrWhiteSpace(ticketNumber))
        {
            throw new ArgumentException("Ticket number is required.", nameof(ticketNumber));
        }

        var now = DateTime.UtcNow;

        return new Ticket(
            id: Guid.CreateVersion7(),
            bookingId: bookingId,
            seatNumber: seatNumber,
            ticketNumber: ticketNumber,
            qrCode: qrCode,
            issuedAtUtc: now);
    }

    public void Cancel(DateTime? cancelledAtUtc = null)
    {
        if (this.Status != TicketStatus.Active)
        {
            return;
        }

        this.Status = TicketStatus.Cancelled;
        this.CancelledAtUtc = cancelledAtUtc ?? DateTime.UtcNow;
    }

    public void MarkRefunded(DateTime? refundedAtUtc = null)
    {
        if (this.Status == TicketStatus.Refunded)
        {
            return;
        }

        this.Status = TicketStatus.Refunded;
        this.CancelledAtUtc = refundedAtUtc ?? DateTime.UtcNow;
    }
}