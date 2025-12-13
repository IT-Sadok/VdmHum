namespace Infrastructure.Repositories;

using Application.Abstractions.Repositories;
using Database;

public class EfUnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await dbContext.SaveChangesAsync(ct);
    }
}