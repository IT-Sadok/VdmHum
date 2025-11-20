namespace Application.Commands.RegisterUser;

using Microsoft.Extensions.Options;
using Domain;
using Domain.Abstractions;
using Options;
using Abstractions.Providers;
using Abstractions.Messaging;
using Contracts;

public sealed class RegisterUserCommandHandler(
    IIdentityService identityService,
    ITokenProvider tokenProvider,
    IOptions<AuthOptions> authOptions,
    IDateTimeProvider dateTimeProvider)
    : ICommandHandler<RegisterUserCommand, AuthResponseModel>
{
    public async Task<Result<AuthResponseModel>> Handle(RegisterUserCommand command, CancellationToken ct)
    {
        var existing = await identityService.FindByEmailAsync(command.Email, ct);

        if (existing is not null)
        {
            return Result.Failure<AuthResponseModel>(UserErrors.EmailNotUnique);
        }

        var user = await identityService.RegisterAsync(
            command.Email,
            command.Password,
            command.PhoneNumber,
            command.FirstName,
            command.LastName,
            ct);

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