namespace Application.Contracts.Bookings;

using Domain.Entities;

public static class BookingMappingExtensions
{
    public static BookingResponseModel ToResponse(
        this Booking booking,
        bool includeTickets = true)
    {
        ArgumentNullException.ThrowIfNull(booking);

        var showtime = booking.Showtime;

        var tickets = includeTickets
            ? booking.Tickets
                .Select(t => new TicketResponseModel(
                    t.Id,
                    t.SeatNumber,
                    t.TicketNumber,
                    t.Status))
                .ToArray()
            : [];

        return new BookingResponseModel(
            Id: booking.Id,
            UserId: booking.UserId,
            ShowtimeId: showtime.ShowtimeId,
            MovieTitle: showtime.MovieTitle,
            CinemaName: showtime.CinemaName,
            HallName: showtime.HallName,
            ShowtimeStartTimeUtc: showtime.StartTimeUtc,
            Status: booking.Status,
            TotalPrice: booking.TotalPrice.Amount,
            Currency: booking.TotalPrice.Currency,
            CreatedAtUtc: booking.CreatedAtUtc,
            ReservationExpiresAtUtc: booking.ReservationExpiresAtUtc,
            Seats: booking.Seats.Select(s => s.SeatNumber).ToArray(),
            Tickets: tickets);
    }
}