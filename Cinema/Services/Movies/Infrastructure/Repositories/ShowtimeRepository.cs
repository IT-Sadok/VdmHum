namespace Infrastructure.Repositories;

using Application.Abstractions.Repositories;
using Application.Contracts;
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
        PagedFilter<ShowtimeFilter> pagedFilter,
        CancellationToken ct)
    {
        var query = dbContext.Showtimes.AsQueryable();

        if (pagedFilter.ModelFilter.MovieId.HasValue)
        {
            var movieId = pagedFilter.ModelFilter.MovieId.Value;
            query = query.Where(s => s.MovieId == movieId);
        }

        if (pagedFilter.ModelFilter.CinemaId.HasValue)
        {
            var cinemaId = pagedFilter.ModelFilter.CinemaId.Value;
            query = query.Where(s => s.CinemaId == cinemaId);
        }

        if (pagedFilter.ModelFilter.HallId.HasValue)
        {
            var hallId = pagedFilter.ModelFilter.HallId.Value;
            query = query.Where(s => s.HallId == hallId);
        }

        if (pagedFilter.ModelFilter.DateFromUtc.HasValue)
        {
            var from = pagedFilter.ModelFilter.DateFromUtc.Value;
            query = query.Where(s => s.StartTimeUtc >= from);
        }

        if (pagedFilter.ModelFilter.DateToUtc.HasValue)
        {
            var to = pagedFilter.ModelFilter.DateToUtc.Value;
            query = query.Where(s => s.StartTimeUtc <= to);
        }

        if (pagedFilter.ModelFilter.Status.HasValue)
        {
            var status = pagedFilter.ModelFilter.Status.Value;
            query = query.Where(s => s.Status == status);
        }

        var totalCount = await query.CountAsync(ct);

        var skip = (pagedFilter.Page - 1) * pagedFilter.PageSize;

        var items = await query
            .OrderBy(s => s.StartTimeUtc)
            .ThenBy(s => s.HallId)
            .Skip(skip)
            .Take(pagedFilter.PageSize)
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