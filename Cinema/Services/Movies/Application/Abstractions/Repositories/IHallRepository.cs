namespace Application.Abstractions.Repositories;

using Domain.Entities;

public interface IHallRepository
{
    Task<Hall?> GetByIdAsync(Guid id, bool asNoTracking, CancellationToken ct);

    void Add(Hall hall, CancellationToken ct);

    void Remove(Hall hall);

    Task<bool> IsNameUniquePerCinemaAsync(
        Guid cinemaId,
        string name,
        Guid? excludeHallId,
        CancellationToken ct);

    Task<(IReadOnlyList<Hall> Items, int TotalCount)> GetPagedAsync(
        Guid? cinemaId,
        string? name,
        int page,
        int pageSize,
        CancellationToken ct);
}