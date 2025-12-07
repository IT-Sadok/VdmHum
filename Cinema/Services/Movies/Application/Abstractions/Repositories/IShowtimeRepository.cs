namespace Application.Abstractions.Repositories;

using Contracts;
using Contracts.Showtimes;
using Domain.Entities;

public interface IShowtimeRepository
{
    Task<Showtime?> GetByIdAsync(Guid id, bool asNoTracking, CancellationToken ct);

    void Add(Showtime showtime, CancellationToken ct);

    void Remove(Showtime showtime);

    Task<(IReadOnlyList<Showtime> Items, int TotalCount)> GetPagedAsync(
        PagedFilter<ShowtimeFilter> pagedFilter,
        CancellationToken ct);

    Task<bool> HasOverlappingAsync(
        Guid hallId,
        DateTime startTimeUtc,
        DateTime endTimeUtc,
        Guid? excludeShowtimeId,
        CancellationToken ct);
}