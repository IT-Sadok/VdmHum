namespace Infrastructure.Repositories;

using Application.Abstractions.Repositories;
using Application.Contracts;
using Application.Contracts.Halls;
using Database;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public sealed class HallRepository(ApplicationDbContext dbContext) : IHallRepository
{
    public async Task<Hall?> GetByIdAsync(
        Guid id,
        bool asNoTracking,
        CancellationToken ct)
    {
        IQueryable<Hall> query = dbContext.Halls;

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(h => h.Id == id, ct);
    }

    public void Add(Hall hall, CancellationToken ct) =>
        dbContext.Halls.Add(hall);

    public void Remove(Hall hall) =>
        dbContext.Halls.Remove(hall);

    public async Task<bool> IsNameUniquePerCinemaAsync(
        Guid cinemaId,
        string name,
        Guid? excludeHallId,
        CancellationToken ct)
    {
        name = name.Trim();

        var query = dbContext.Halls
            .Where(h => h.CinemaId == cinemaId && h.Name == name);

        if (excludeHallId.HasValue)
        {
            var id = excludeHallId.Value;
            query = query.Where(h => h.Id != id);
        }

        return !await query.AnyAsync(ct);
    }

    public async Task<(IReadOnlyList<Hall> Items, int TotalCount)> GetPagedAsync(
        PagedFilter<HallFilter> pagedFilter,
        CancellationToken ct)
    {
        var query = dbContext.Halls.AsQueryable();

        if (pagedFilter.ModelFilter.CinemaId.HasValue)
        {
            var id = pagedFilter.ModelFilter.CinemaId.Value;
            query = query.Where(h => h.CinemaId == id);
        }

        if (!string.IsNullOrWhiteSpace(pagedFilter.ModelFilter.Name))
        {
            var n = pagedFilter.ModelFilter.Name.Trim();
            query = query.Where(h => h.Name.Contains(n));
        }

        var totalCount = await query.CountAsync(ct);

        var skip = (pagedFilter.Page - 1) * pagedFilter.PageSize;

        var items = await query
            .OrderBy(h => h.CinemaId)
            .ThenBy(h => h.Name)
            .Skip(skip)
            .Take(pagedFilter.PageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }
}