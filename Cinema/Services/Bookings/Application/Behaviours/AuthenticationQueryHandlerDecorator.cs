namespace Application.Behaviours;

using Abstractions;
using Abstractions.Services;
using Errors;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class AuthenticationQueryHandlerDecorator<TQuery, TResult>(
    IQueryHandler<TQuery, TResult> inner,
    IUserContextService userContextService)
    : IQueryHandler<TQuery, TResult>
    where TQuery : IQuery<TResult>
{
    public async Task<Result<TResult>> HandleAsync(
        TQuery query,
        CancellationToken ct)
    {
        if (query is not IAuthenticationRequired)
        {
            return await inner.HandleAsync(query, ct);
        }

        var userContext = userContextService.Get();

        if (!userContext.IsAuthenticated || userContext.UserId is null)
        {
            return Result.Failure<TResult>(CommonErrors.Unauthorized);
        }

        return await inner.HandleAsync(query, ct);
    }
}