namespace Infrastructure.Repositories;

using Application.Abstractions.Repositories;
using Application.Contracts.Cinemas;
using Database;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

public sealed class CinemaRepository(ApplicationDbContext dbContext) : ICinemaRepository
{
    public async Task<Cinema?> GetByIdAsync(
        Guid id,
        bool asNoTracking,
        CancellationToken ct)
    {
        IQueryable<Cinema> query = dbContext
            .Cinemas
            .Include(c => c.Halls);

        if (asNoTracking)
        {
            query = query.AsNoTracking();
        }

        return await query.FirstOrDefaultAsync(c => c.Id == id, ct);
    }

    public void Add(Cinema cinema, CancellationToken ct) =>
        dbContext.Cinemas.Add(cinema);

    public void Remove(Cinema cinema) =>
        dbContext.Cinemas.Remove(cinema);

    public async Task<bool> IsNameUniquePerCityAsync(
        string name,
        string city,
        Guid? excludeCinemaId,
        CancellationToken ct)
    {
        name = name.Trim();
        city = city.Trim();

        var query = dbContext.Cinemas.AsQueryable();

        query = query.Where(c =>
            c.Name == name &&
            c.City == city);

        if (excludeCinemaId.HasValue)
        {
            var id = excludeCinemaId.Value;
            query = query.Where(c => c.Id != id);
        }

        return !await query.AnyAsync(ct);
    }

    public async Task<(IReadOnlyList<Cinema> Items, int TotalCount)> GetPagedAsync(
        CinemaFilter filter,
        int page,
        int pageSize,
        CancellationToken ct)
    {
        var query = dbContext.Cinemas.AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            var name = filter.Name.Trim();
            query = query.Where(c => c.Name.Contains(name));
        }

        if (!string.IsNullOrWhiteSpace(filter.City))
        {
            var city = filter.City.Trim();
            query = query.Where(c => c.City.Contains(city));
        }

        var totalCount = await query.CountAsync(ct);

        var skip = (page - 1) * pageSize;

        var items = await query
            .OrderBy(c => c.City)
            .ThenBy(c => c.Name)
            .Skip(skip)
            .Take(pageSize)
            .ToListAsync(ct);

        return (items, totalCount);
    }
}