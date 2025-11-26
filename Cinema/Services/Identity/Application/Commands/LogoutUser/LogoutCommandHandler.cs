namespace Application.Commands.LogoutUser;

using Abstractions.Providers;
using Abstractions.Messaging;
using Domain;
using Domain.Abstractions;

public class LogoutCommandHandler(
    IIdentityService identityService,
    IUserContext userContext)
    : ICommandHandler<LogoutUserCommand, Result>
{
    public async Task<Result<Result>> HandleAsync(LogoutUserCommand command, CancellationToken ct)
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