namespace Shared.Contracts.Abstractions;

using Core;
using Microsoft.Extensions.DependencyInjection;

public sealed class Mediator(IServiceScopeFactory scopeFactory) : IMediator
{
    public async Task<Result> ExecuteCommandAsync<TCommand>(
        TCommand command,
        CancellationToken cancellationToken)
        where TCommand : ICommand
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var commandHandler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand>>();

        return await commandHandler.HandleAsync(command, cancellationToken);
    }

    public async Task<Result<TResponse>> ExecuteCommandAsync<TCommand, TResponse>(
        TCommand command,
        CancellationToken cancellationToken)
        where TCommand : ICommand<TResponse>
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var commandHandler = scope.ServiceProvider.GetRequiredService<ICommandHandler<TCommand, TResponse>>();

        return await commandHandler.HandleAsync(command, cancellationToken);
    }

    public async Task<Result<TResponse>> ExecuteQueryAsync<TQuery, TResponse>(
        TQuery query,
        CancellationToken cancellationToken)
        where TQuery : IQuery<TResponse>
    {
        await using var scope = scopeFactory.CreateAsyncScope();
        var queryHandler = scope.ServiceProvider.GetRequiredService<IQueryHandler<TQuery, TResponse>>();

        return await queryHandler.HandleAsync(query, cancellationToken);
    }
}