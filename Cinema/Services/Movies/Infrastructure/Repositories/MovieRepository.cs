namespace Infrastructure.Repositories;

using Application.Abstractions.Repositories;
using Application.Contracts;
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
        PagedFilter<MovieFilter> pagedFilter,
        CancellationToken ct)
    {
        var query = dbContext.Movies.AsQueryable();

        if (pagedFilter.ModelFilter.Genres is { Count: > 0 })
        {
            var genres = pagedFilter.ModelFilter.Genres;
            query = query.Where(m => m.MovieGenres.Any(mg => genres.Contains(mg.Genre)));
        }

        if (pagedFilter.ModelFilter.MinDurationMinutes.HasValue)
        {
            var min = pagedFilter.ModelFilter.MinDurationMinutes.Value;
            query = query.Where(m =>
                m.DurationMinutes.HasValue &&
                m.DurationMinutes.Value >= min);
        }

        if (pagedFilter.ModelFilter.MaxDurationMinutes.HasValue)
        {
            var max = pagedFilter.ModelFilter.MaxDurationMinutes.Value;
            query = query.Where(m =>
                m.DurationMinutes.HasValue &&
                m.DurationMinutes.Value <= max);
        }

        if (pagedFilter.ModelFilter.MinAgeRating.HasValue)
        {
            var min = pagedFilter.ModelFilter.MinAgeRating.Value;
            query = query.Where(m =>
                m.AgeRating.HasValue &&
                m.AgeRating.Value >= min);
        }

        if (pagedFilter.ModelFilter.MaxAgeRating.HasValue)
        {
            var max = pagedFilter.ModelFilter.MaxAgeRating.Value;
            query = query.Where(m =>
                m.AgeRating.HasValue &&
                m.AgeRating.Value <= max);
        }

        if (pagedFilter.ModelFilter.Status.HasValue)
        {
            var status = pagedFilter.ModelFilter.Status.Value;
            query = query.Where(m => m.Status == status);
        }

        var totalCount = await query.CountAsync(ct);

        var skip = (pagedFilter.Page - 1) * pagedFilter.PageSize;

        var items = await query
            .OrderBy(m => m.Title)
            .Skip(skip)
            .Take(pagedFilter.PageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }
}