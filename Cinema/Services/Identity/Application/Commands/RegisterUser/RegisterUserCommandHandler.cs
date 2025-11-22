namespace Application.Commands.RegisterUser;

using Microsoft.Extensions.Options;
using Domain;
using Domain.Abstractions;
using Domain.Constants;
using Options;
using Abstractions.Providers;
using Abstractions.Messaging;
using Contracts;

public sealed class RegisterUserCommandHandler(
    IIdentityService identityService,
    ITokenProvider tokenProvider,
    IOptions<JwtOptions> jwtOptions,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterUserCommand, AuthResponseModel>
{
    private readonly JwtOptions _options = jwtOptions.Value;

    public async Task<Result<AuthResponseModel>> Handle(RegisterUserCommand command, CancellationToken ct)
    {
        var existing = await identityService.FindByEmailAsync(command.Email, ct);

        if (existing is not null)
        {
            return Result.Failure<AuthResponseModel>(UserErrors.EmailNotUnique);
        }

        return await unitOfWork.ExecuteInTransactionAsync(
            async innerCt =>
            {
                var user = await identityService.RegisterAsync(
                    command.Email,
                    command.Password,
                    command.PhoneNumber,
                    command.FirstName,
                    command.LastName,
                    innerCt);

                await identityService.AddToRoleAsync(user, RoleNames.User, innerCt);

                var accessToken = tokenProvider.CreateAccessToken(user);
                var refreshToken = tokenProvider.CreateRefreshToken(user);

                var refreshLifetime = TimeSpan.FromDays(this._options.RefreshTokenLifetimeDays);
                var expiresAtUtc = dateTimeProvider.UtcNow.Add(refreshLifetime);

                await identityService.StoreRefreshTokenAsync(
                    user.Id,
                    refreshToken,
                    expiresAtUtc,
                    innerCt);

                return new AuthResponseModel(user.Id, accessToken, refreshToken);
            },
            ct);
    }
}