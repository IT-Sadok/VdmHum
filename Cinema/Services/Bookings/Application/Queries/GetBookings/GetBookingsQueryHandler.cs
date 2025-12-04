namespace Application.Queries.GetBookings;

using Abstractions.Messaging;
using Abstractions.Repositories;
using Contracts.Bookings;
using Domain.Abstractions;
using Domain.Entities;

public sealed class GetBookingsQueryHandler(
    IBookingRepository bookingRepository)
    : IQueryHandler<GetBookingsQuery, PagedBookingsResponseModel>
{
    public async Task<Result<PagedBookingsResponseModel>> HandleAsync(
        GetBookingsQuery query,
        CancellationToken ct)
    {
        var bookingFilter = new BookingFilter(
            UserId: query.UserId,
            Status: query.Status);

        var (bookings, totalCount) = await bookingRepository.GetPagedAsync(
            bookingFilter,
            query.Page,
            query.PageSize,
            ct);

        var responseItems = bookings
            .Select(MapToResponse)
            .ToArray();

        var response = new PagedBookingsResponseModel(
            Page: query.Page,
            PageSize: query.PageSize,
            TotalCount: totalCount,
            Items: responseItems);

        return response;
    }

    private static BookingResponseModel MapToResponse(Booking booking)
    {
        var showtime = booking.Showtime;

        var tickets = booking.Tickets
            .Select(t => new TicketResponseModel(
                t.Id,
                t.SeatNumber,
                t.TicketNumber,
                t.Status))
            .ToArray();

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
            Seats: booking.Seats.ToArray(),
            Tickets: tickets);
    }
}