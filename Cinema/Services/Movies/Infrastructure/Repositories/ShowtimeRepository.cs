namespace Infrastructure.Repositories;

using Application.Abstractions.Repositories;
using Application.Contracts.Showtimes;
using Database;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public sealed class ShowtimeRepository(ApplicationDbContext dbContext) : IShowtimeRepository
{
    public async Task<Showtime?> GetByIdAsync(
        Guid id,
        bool asNoTracking,
        CancellationToken ct)
    {
        IQueryable<Showtime> query = dbContext.Showtimes;

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(s => s.Id == id, ct);
    }

    public void Add(Showtime showtime, CancellationToken ct) =>
        dbContext.Showtimes.Add(showtime);

    public void Remove(Showtime showtime) =>
        dbContext.Showtimes.Remove(showtime);

    public async Task<(IReadOnlyList<Showtime> Items, int TotalCount)> GetPagedAsync(
        ShowtimeFilter filter,
        int page,
        int pageSize,
        CancellationToken ct)
    {
        var query = dbContext.Showtimes.AsQueryable();

        if (filter.MovieId.HasValue)
        {
            var movieId = filter.MovieId.Value;
            query = query.Where(s => s.MovieId == movieId);
        }

        if (filter.CinemaId.HasValue)
        {
            var cinemaId = filter.CinemaId.Value;
            query = query.Where(s => s.CinemaId == cinemaId);
        }

        if (filter.HallId.HasValue)
        {
            var hallId = filter.HallId.Value;
            query = query.Where(s => s.HallId == hallId);
        }

        if (filter.DateFromUtc.HasValue)
        {
            var from = filter.DateFromUtc.Value;
            query = query.Where(s => s.StartTimeUtc >= from);
        }

        if (filter.DateToUtc.HasValue)
        {
            var to = filter.DateToUtc.Value;
            query = query.Where(s => s.StartTimeUtc <= to);
        }

        if (filter.Status.HasValue)
        {
            var status = filter.Status.Value;
            query = query.Where(s => s.Status == status);
        }

        var totalCount = await query.CountAsync(ct);

        var skip = (page - 1) * pageSize;

        var items = await query
            .OrderBy(s => s.StartTimeUtc)
            .ThenBy(s => s.HallId)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }

    public async Task<bool> HasOverlappingAsync(
        Guid hallId,
        DateTime startTimeUtc,
        DateTime endTimeUtc,
        Guid? excludeShowtimeId,
        CancellationToken ct)
    {
        var query = dbContext.Showtimes
            .Where(s => s.HallId == hallId &&
                        s.StartTimeUtc < endTimeUtc &&
                        s.EndTimeUtc > startTimeUtc);

        if (excludeShowtimeId.HasValue)
        {
            var id = excludeShowtimeId.Value;
            query = query.Where(s => s.Id != id);
        }

        return await query.AnyAsync(ct);
    }
}