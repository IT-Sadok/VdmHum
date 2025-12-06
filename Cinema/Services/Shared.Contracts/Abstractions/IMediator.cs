namespace Shared.Contracts.Abstractions;

using Core;

public interface IMediator
{
    Task<Result<TResult>> Send<TResult>(
        ICommand<TResult> command,
        CancellationToken ct = default);
    
    Task<Result> Send(
        ICommand command,
        CancellationToken ct = default);

    Task<Result<TResult>> Send<TResult>(
        IQuery<TResult> query,
        CancellationToken ct = default);
}