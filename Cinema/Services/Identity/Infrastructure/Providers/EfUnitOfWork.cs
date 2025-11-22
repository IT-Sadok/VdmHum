namespace Infrastructure.Providers;

using Application.Abstractions.Providers;
using Persistence;

public sealed class EfUnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public async Task<T> ExecuteInTransactionAsync<T>(
        Func<CancellationToken, Task<T>> action,
        CancellationToken ct = default)
    {
        await using var transaction = await dbContext.Database.BeginTransactionAsync(ct);

        try
        {
            var result = await action(ct);

            await transaction.CommitAsync(ct);

            return result;
        }
        catch
        {
            await transaction.RollbackAsync(ct);
            throw;
        }
    }
}