namespace Application.Commands.LoginUser;

using Microsoft.Extensions.Options;
using Domain;
using Domain.Abstractions;
using Options;
using Abstractions.Providers;
using Abstractions.Messaging;
using Contracts;

public sealed class LoginUserCommandHandler(
    IIdentityService identityService,
    ITokenProvider tokenProvider,
    IOptions<AuthOptions> authOptions,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<LoginUserCommand, AuthResponseModel>
{
    public async Task<Result<AuthResponseModel>> Handle(LoginUserCommand command, CancellationToken ct)
    {
        var user = await identityService.FindByEmailAsync(command.Email, ct);

        if (user is null)
        {
            return Result.Failure<AuthResponseModel>(UserErrors.InvalidCredentials);
        }

        var validPassword = await identityService.CheckPasswordAsync(user, command.Password, ct);

        if (!validPassword)
        {
            return Result.Failure<AuthResponseModel>(UserErrors.InvalidCredentials);
        }

        var accessToken = tokenProvider.CreateAccessToken(user);
        var refreshToken = tokenProvider.CreateRefreshToken(user);

        var refreshLifetime = TimeSpan.FromDays(authOptions.Value.RefreshTokenLifetimeDays);
        var expiresAtUtc = dateTimeProvider.UtcNow.Add(refreshLifetime);

        await identityService.StoreRefreshTokenAsync(
            user.Id,
            refreshToken,
            expiresAtUtc,
            ct);

        return new AuthResponseModel(user.Id, accessToken, refreshToken);
    }
}