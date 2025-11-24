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
    IUserContext userContext,
    ITokenProvider tokenProvider,
    IOptions<JwtOptions> jwtOptions,
    IDateTimeProvider dateTimeProvider,
    IUnitOfWork unitOfWork)
    : ICommandHandler<RegisterUserCommand, AuthResponseModel>
{
    private readonly JwtOptions _options = jwtOptions.Value;

    public async Task<Result<AuthResponseModel>> HandleAsync(RegisterUserCommand command, CancellationToken ct)
    {
        if (userContext.IsAuthenticated)
        {
            return Result.Failure<AuthResponseModel>(UserErrors.AlreadyAuthenticated);
        }

        var existing = await identityService.FindByEmailAsync(command.Email, ct);

        if (existing is not null)
        {
            return Result.Failure<AuthResponseModel>(UserErrors.EmailNotUnique);
        }

        await using var transaction = await unitOfWork.BeginTransactionAsync(ct);

        var user = await identityService.RegisterAsync(
            command.Email,
            command.Password,
            command.PhoneNumber,
            command.FirstName,
            command.LastName,
            ct);

        await identityService.AddToRoleAsync(user, RoleNames.User, ct);

        var accessToken = tokenProvider.CreateAccessToken(user);
        var refreshToken = tokenProvider.CreateRefreshToken(user);

        var refreshLifetime = TimeSpan.FromDays(this._options.RefreshTokenLifetimeDays);
        var expiresAtUtc = dateTimeProvider.UtcNow.Add(refreshLifetime);

        await identityService.StoreRefreshTokenAsync(
            user.Id,
            refreshToken,
            expiresAtUtc,
            ct);

        await transaction.CommitAsync(ct);

        return new AuthResponseModel(user.Id, accessToken, refreshToken);
    }
}