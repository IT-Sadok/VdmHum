namespace Shared.Contracts.Abstractions;

using Core;

public sealed class Mediator(IServiceProvider serviceProvider) : IMediator
{
    public Task<Result<TResult>> Send<TResult>(
        ICommand<TResult> command,
        CancellationToken ct = default)
    {
        return InvokeHandler<TResult>(typeof(ICommandHandler<,>), command, ct);
    }

    public Task<Result<TResult>> Send<TResult>(
        IQuery<TResult> query,
        CancellationToken ct = default)
    {
        return InvokeHandler<TResult>(typeof(IQueryHandler<,>), query, ct);
    }

    private Task<Result<TResult>> InvokeHandler<TResult>(
        Type openGenericHandlerType,
        object message,
        CancellationToken ct)
    {
        var messageType = message.GetType();
        var resultType = typeof(TResult);

        var closedHandlerType = openGenericHandlerType
            .MakeGenericType(messageType, resultType);

        var handler = serviceProvider.GetService(closedHandlerType);

        return ((dynamic)handler!).HandleAsync((dynamic)message, ct);
    }
}