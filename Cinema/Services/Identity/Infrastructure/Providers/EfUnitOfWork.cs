namespace Infrastructure.Providers;

using Microsoft.EntityFrameworkCore.Storage;
using Application.Abstractions.Providers;
using Persistence;

public sealed class EfUnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    public async Task<IUnitOfWorkTransaction> BeginTransactionAsync(CancellationToken ct = default)
    {
        var tx = await dbContext.Database.BeginTransactionAsync(ct);
        return new EfUnitOfWorkTransaction(tx);
    }

    private sealed class EfUnitOfWorkTransaction(IDbContextTransaction transaction)
        : IUnitOfWorkTransaction
    {
        public Task CommitAsync(CancellationToken ct = default) =>
            transaction.CommitAsync(ct);

        public Task RollbackAsync(CancellationToken ct = default) =>
            transaction.RollbackAsync(ct);

        public ValueTask DisposeAsync() =>
            transaction.DisposeAsync();
    }
}