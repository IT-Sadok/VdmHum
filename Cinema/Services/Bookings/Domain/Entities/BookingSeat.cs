namespace Domain.Entities;

public sealed class BookingSeat
{
    private BookingSeat()
    {
    }

    private BookingSeat(
        Guid id,
        Guid bookingId,
        Guid showtimeId,
        int seatNumber)
    {
        this.Id = id;
        this.BookingId = bookingId;
        this.ShowtimeId = showtimeId;
        this.SeatNumber = seatNumber;
    }

    public Guid Id { get; private set; }

    public Guid BookingId { get; private set; }

    public Guid ShowtimeId { get; private set; }

    public int SeatNumber { get; private set; }

    internal static BookingSeat Create(
        Guid bookingId,
        Guid showtimeId,
        int seatNumber)
    {
        if (bookingId == Guid.Empty)
        {
            throw new ArgumentException("BookingId cannot be empty.", nameof(bookingId));
        }

        if (seatNumber <= 0)
        {
            throw new ArgumentException("Seat number must be positive.", nameof(seatNumber));
        }

        return new BookingSeat(
            id: Guid.CreateVersion7(),
            bookingId: bookingId,
            showtimeId: showtimeId,
            seatNumber: seatNumber);
    }
}