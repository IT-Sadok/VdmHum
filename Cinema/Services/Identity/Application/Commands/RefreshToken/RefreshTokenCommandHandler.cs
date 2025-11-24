namespace Application.Commands.RefreshToken;

using Options;
using Microsoft.Extensions.Options;
using Domain;
using Domain.Abstractions;
using Abstractions.Providers;
using Abstractions.Messaging;
using Contracts;

public class RefreshTokenCommandHandler(
    IIdentityService identityService,
    ITokenProvider tokenProvider,
    IOptions<JwtOptions> authOptions,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<RefreshTokenCommand, AuthResponseModel>
{
    public async Task<Result<AuthResponseModel>> HandleAsync(RefreshTokenCommand command, CancellationToken ct)
    {
        var user = await identityService.GetUserByRefreshTokenAsync(command.RefreshToken, ct);

        if (user is null || !await identityService.TryRevokeRefreshTokenAsync(command.RefreshToken, ct))
        {
            return Result.Failure<AuthResponseModel>(UserErrors.InvalidRefreshToken);
        }

        var newAccessToken = tokenProvider.CreateAccessToken(user);
        var newRefreshToken = tokenProvider.CreateRefreshToken();

        var refreshLifetime = TimeSpan.FromDays(authOptions.Value.RefreshTokenLifetimeDays);
        var expiresAtUtc = dateTimeProvider.UtcNow.Add(refreshLifetime);

        await identityService.StoreRefreshTokenAsync(
            user.Id,
            newRefreshToken,
            expiresAtUtc,
            ct);

        return new AuthResponseModel(user.Id, newAccessToken, newRefreshToken);
    }
}