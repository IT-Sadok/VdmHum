namespace Application.Abstractions.Repositories;

using Contracts;
using Contracts.Cinemas;
using Domain.Entities;

public interface ICinemaRepository
{
    Task<Cinema?> GetByIdAsync(Guid id, bool asNoTracking, CancellationToken ct);

    void Add(Cinema cinema, CancellationToken ct);

    void Remove(Cinema cinema);

    Task<bool> IsNameUniquePerCityAsync(
        string name,
        string city,
        Guid? excludeCinemaId,
        CancellationToken ct);

    Task<(IReadOnlyList<Cinema> Items, int TotalCount)> GetPagedAsync(
        PagedFilter<CinemaFilter> pagedFilter,
        CancellationToken ct);
}