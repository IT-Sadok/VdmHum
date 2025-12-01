namespace Infrastructure.Repositories;

using Application.Abstractions.Repositories;
using Application.Contracts.Movies;
using Database;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public sealed class MovieRepository(ApplicationDbContext dbContext) : IMovieRepository
{
    public async Task<Movie?> GetByIdAsync(
        Guid id,
        bool asNoTracking,
        CancellationToken ct)
    {
        IQueryable<Movie> query = dbContext.Movies;

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(m => m.Id == id, ct);
    }

    public void Add(Movie movie, CancellationToken ct) =>
        dbContext.Movies.Add(movie);

    public void Remove(Movie movie) =>
        dbContext.Movies.Remove(movie);

    public async Task<bool> IsTitleUniqueAsync(
        string title,
        Guid? excludeMovieId,
        CancellationToken ct)
    {
        title = title.Trim();

        var query = dbContext.Movies
            .Where(m => m.Title == title);

        if (excludeMovieId.HasValue)
        {
            var id = excludeMovieId.Value;
            query = query.Where(m => m.Id != id);
        }

        return !await query.AnyAsync(ct);
    }

    public async Task<(IReadOnlyList<Movie> Items, int TotalCount)> GetPagedAsync(
        MovieFilter filter,
        int page,
        int pageSize,
        CancellationToken ct)
    {
        var query = dbContext.Movies.AsQueryable();

        if (filter.Genres is { Count: > 0 })
        {
            var genres = filter.Genres;
            query = query.Where(m => m.MovieGenres.Any(mg => genres.Contains(mg.Genre)));
        }

        if (filter.MinDurationMinutes.HasValue)
        {
            var min = filter.MinDurationMinutes.Value;
            query = query.Where(m =>
                m.DurationMinutes.HasValue &&
                m.DurationMinutes.Value >= min);
        }

        if (filter.MaxDurationMinutes.HasValue)
        {
            var max = filter.MaxDurationMinutes.Value;
            query = query.Where(m =>
                m.DurationMinutes.HasValue &&
                m.DurationMinutes.Value <= max);
        }

        if (filter.MinAgeRating.HasValue)
        {
            var min = filter.MinAgeRating.Value;
            query = query.Where(m =>
                m.AgeRating.HasValue &&
                m.AgeRating.Value >= min);
        }

        if (filter.MaxAgeRating.HasValue)
        {
            var max = filter.MaxAgeRating.Value;
            query = query.Where(m =>
                m.AgeRating.HasValue &&
                m.AgeRating.Value <= max);
        }

        if (filter.Status.HasValue)
        {
            var status = filter.Status.Value;
            query = query.Where(m => m.Status == status);
        }

        var totalCount = await query.CountAsync(ct);

        var skip = (page - 1) * pageSize;

        var items = await query
            .OrderBy(m => m.Title)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }
}