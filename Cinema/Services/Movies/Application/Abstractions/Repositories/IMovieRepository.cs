namespace Application.Abstractions.Repositories;

using Contracts.Movies;
using Domain.Entities;

public interface IMovieRepository
{
    Task<Movie?> GetByIdAsync(Guid id, bool asNoTracking, CancellationToken ct);

    void Add(Movie movie, CancellationToken ct);

    void Remove(Movie movie);

    Task<bool> IsTitleUniqueAsync(string title, Guid? excludeMovieId, CancellationToken ct);

    Task<(IReadOnlyList<Movie> Items, int TotalCount)> GetPagedAsync(
        MovieFilter filter,
        int page,
        int pageSize,
        CancellationToken ct);
}