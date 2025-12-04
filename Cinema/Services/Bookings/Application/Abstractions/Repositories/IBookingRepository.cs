namespace Application.Abstractions.Repositories;

using Contracts.Bookings;
using Domain.Entities;

public interface IBookingRepository
{
    Task<Booking?> GetByIdAsync(Guid id, CancellationToken ct);

    Task<(IReadOnlyList<Booking> Items, int TotalCount)> GetPagedAsync(
        BookingFilter filter,
        int page,
        int pageSize,
        CancellationToken ct);

    void Add(Booking booking);

    Task<bool> AreSeatsAvailableAsync(
        Guid showtimeId,
        IReadOnlyCollection<int> seats,
        CancellationToken ct);
}