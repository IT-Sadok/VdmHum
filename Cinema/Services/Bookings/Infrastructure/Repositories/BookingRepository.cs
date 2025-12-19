namespace Infrastructure.Repositories;

using Application.Abstractions.Repositories;
using Application.Contracts;
using Application.Contracts.Bookings;
using Database;
using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;

public class BookingRepository(ApplicationDbContext dbContext) : IBookingRepository
{
    public async Task<Booking?> GetByIdAsync(
        Guid id,
        bool asNoTracking,
        CancellationToken ct)
    {
        IQueryable<Booking> query = dbContext
            .Bookings
            .Include(b => b.Tickets)
            .Include(b => b.Seats);

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public async Task<(IReadOnlyList<Booking> Items, int TotalCount)> GetPagedAsync(
        PagedFilter<BookingFilter> pagedFilter,
        CancellationToken ct)
    {
        var query = dbContext.Bookings.AsQueryable();

        if (pagedFilter.ModelFilter.UserId is not null)
        {
            query = query.Where(b => b.UserId == pagedFilter.ModelFilter.UserId);
        }

        if (pagedFilter.ModelFilter.Status is not null)
        {
            query = query.Where(b => b.Status == pagedFilter.ModelFilter.Status);
        }

        var totalCount = await query.CountAsync(ct);

        var skip = (pagedFilter.Page - 1) * pagedFilter.PageSize;

        var items = await query
            .OrderBy(c => c.Id)
            .Skip(skip)
            .Take(pagedFilter.PageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    public void Add(Booking booking) =>
        dbContext.Bookings.Add(booking);

    public async Task<bool> AreSeatsAvailableAsync(
        Guid showtimeId,
        IReadOnlyCollection<int> seats,
        CancellationToken ct)
    {
        if (seats is null || seats.Count == 0)
        {
            throw new ArgumentException("At least one seat must be provided.", nameof(seats));
        }

        var requestedSeats = seats.ToHashSet();
        var now = DateTime.UtcNow;

        var hasConflict = await this.BuildConflictingSeatsQuery(showtimeId, requestedSeats, now)
            .AnyAsync(ct);

        return !hasConflict;
    }

    private IQueryable<BookingSeat> BuildConflictingSeatsQuery(
        Guid showtimeId,
        HashSet<int> requestedSeats,
        DateTime now)
    {
        return
            from seat in dbContext.BookingSeats.AsNoTracking()
            join booking in dbContext.Bookings.AsNoTracking()
                on seat.BookingId equals booking.Id
            where seat.ShowtimeId == showtimeId
                  && requestedSeats.Contains(seat.SeatNumber)
                  && (
                      (booking.Status == BookingStatus.PendingPayment &&
                       booking.ReservationExpiresAtUtc > now)
                      || booking.Status == BookingStatus.Confirmed
                      || booking.Status == BookingStatus.RefundPending
                  )
            select seat;
    }
}