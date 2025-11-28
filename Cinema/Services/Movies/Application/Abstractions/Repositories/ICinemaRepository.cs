namespace Application.Abstractions.Repositories;

using Contracts.Cinemas;
using Domain.Entities;

public interface ICinemaRepository
{
    Task<Cinema?> GetByIdAsync(Guid id, CancellationToken ct);

    void Add(Cinema cinema, CancellationToken ct);

    void Remove(Cinema cinema);

    Task<bool> IsNameUniqueInCityAsync(
        string name,
        string city,
        Guid? excludeCinemaId,
        CancellationToken ct);

    Task<(IReadOnlyList<Cinema> Items, int TotalCount)> GetPagedAsync(
        CinemaFilter filter,
        int page,
        int pageSize,
        CancellationToken ct);
}