namespace Application.Queries.GetBookings;

using Abstractions;
using Abstractions.Repositories;
using Contracts;
using Contracts.Bookings;
using Domain.Abstractions;
using Domain.Entities;

public sealed class GetBookingsQueryHandler(
    IBookingRepository bookingRepository)
    : IQueryHandler<GetBookingsQuery, PagedResponse<BookingResponseModel>>
{
    public async Task<Result<PagedResponse<BookingResponseModel>>> HandleAsync(
        GetBookingsQuery query,
        CancellationToken ct)
    {
        var (bookings, totalCount) = await bookingRepository.GetPagedAsync(
            query.Filter,
            ct);

        var responseItems = bookings
            .Select(MapToResponse)
            .ToArray();

        var response = new PagedResponse<BookingResponseModel>(
            Page: query.Filter.Page,
            PageSize: query.Filter.PageSize,
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