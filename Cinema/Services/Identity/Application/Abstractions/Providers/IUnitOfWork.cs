namespace Application.Abstractions.Providers;

public interface IUnitOfWork
{
    Task<IUnitOfWorkTransaction> BeginTransactionAsync(CancellationToken ct = default);
}