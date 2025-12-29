namespace Application.Abstractions.Repositories;

using Contracts;
using Contracts.Bookings;
using Domain.Entities;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, bool asNoTracking, CancellationToken ct);

    Task<(IReadOnlyList<Booking> Items, int TotalCount)> GetPagedAsync(
        PagedFilter<BookingFilter> pagedFilter,
        CancellationToken ct);

    void Add(Booking booking);

    Task<bool> AreSeatsAvailableAsync(
        Guid showtimeId,
        IReadOnlyCollection<int> seats,
        CancellationToken ct);

    Task<IReadOnlyList<Booking>> GetExpiredReservationsAsync(DateTime utcNow, CancellationToken ct);
}