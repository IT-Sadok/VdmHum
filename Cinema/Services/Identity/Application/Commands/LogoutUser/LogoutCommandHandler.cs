namespace Application.Commands.LogoutUser;

using Abstractions.Providers;
using Errors;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public class LogoutCommandHandler(
    IIdentityService identityService,
    IUserContext userContext)
    : ICommandHandler<LogoutUserCommand>
{
    public async Task<Result> HandleAsync(LogoutUserCommand command, CancellationToken ct)
    {
        if (!userContext.IsAuthenticated || userContext.UserId is null)
        {
            return Result.Failure(UserErrors.Unauthorized);
        }

        if (!await identityService.TryRevokeRefreshTokenAsync(command.RefreshToken, ct))
        {
            return Result.Failure(UserErrors.InvalidRefreshToken);
        }

        return Result.Success();
    }
}