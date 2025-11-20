namespace Application.Commands.LogoutUser;

using Abstractions.Providers;
using Abstractions.Messaging;
using Domain;
using Domain.Abstractions;

public class LogoutCommandHandler(
    IIdentityService identityService,
    ICurrentUserService currentUserService)
    : ICommandHandler<LogoutUserCommand, Result>
{
    public async Task<Result<Result>> Handle(LogoutUserCommand command, CancellationToken ct)
    {
        if (!currentUserService.IsAuthenticated || currentUserService.UserId is null)
        {
            return Result.Failure(UserErrors.Unauthorized);
        }

        await identityService.RevokeRefreshTokenAsync(command.RefreshToken, ct);

        return Result.Success();
    }
}