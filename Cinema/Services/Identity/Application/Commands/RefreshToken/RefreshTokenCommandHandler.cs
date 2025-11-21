namespace Application.Commands.RefreshToken;

using Domain;
using Domain.Abstractions;
using Abstractions.Providers;
using Abstractions.Messaging;
using Contracts;

public class RefreshTokenCommandHandler(
    IIdentityService identityService,
    ITokenProvider tokenProvider)
    : ICommandHandler<RefreshTokenCommand, AuthResponseModel>
{
    public async Task<Result<AuthResponseModel>> Handle(RefreshTokenCommand command, CancellationToken ct)
    {
        var user = await identityService.GetUserByRefreshTokenAsync(command.RefreshToken, ct);

        if (user is null)
        {
            return Result.Failure<AuthResponseModel>(UserErrors.InvalidRefreshToken);
        }

        var newAccessToken = tokenProvider.CreateAccessToken(user);

        return new AuthResponseModel(user.Id, newAccessToken, command.RefreshToken);
    }
}