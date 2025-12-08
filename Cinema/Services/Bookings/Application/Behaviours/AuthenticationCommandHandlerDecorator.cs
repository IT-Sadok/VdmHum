namespace Application.Behaviours;

using Abstractions;
using Abstractions.Services;
using Errors;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class AuthenticationCommandHandlerDecorator<TCommand, TResult>(
    ICommandHandler<TCommand, TResult> inner,
    IUserContextService userContextService)
    : ICommandHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
    public async Task<Result<TResult>> HandleAsync(
        TCommand command,
        CancellationToken ct)
    {
        if (command is not IAuthenticationRequired)
        {
            return await inner.HandleAsync(command, ct);
        }

        var userContext = userContextService.Get();

        if (!userContext.IsAuthenticated || userContext.UserId is null)
        {
            return Result.Failure<TResult>(CommonErrors.Unauthorized);
        }

        return await inner.HandleAsync(command, ct);
    }
}