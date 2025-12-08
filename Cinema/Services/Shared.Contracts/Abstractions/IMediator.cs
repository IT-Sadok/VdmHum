namespace Shared.Contracts.Abstractions;

using Core;

public interface IMediator
{
    Task<Result> ExecuteCommandAsync<TCommand>(
        TCommand command,
        CancellationToken cancellationToken)
        where TCommand : ICommand;

    Task<Result<TResponse>> ExecuteCommandAsync<TCommand, TResponse>(
        TCommand command,
        CancellationToken cancellationToken)
        where TCommand : ICommand<TResponse>;

    Task<Result<TResponse>> ExecuteQueryAsync<TQuery, TResponse>(TQuery query,
        CancellationToken cancellationToken)
        where TQuery : IQuery<TResponse>;
}