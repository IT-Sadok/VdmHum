namespace Application.Commands.UpdateProfile;

using Domain;
using Domain.Abstractions;
using Abstractions.Providers;
using Abstractions.Messaging;

public class UpdateProfileCommandHandler(
    IIdentityService identityService,
    IUserContext userContext)
    : ICommandHandler<UpdateProfileCommand, Guid>
{
    public async Task<Result<Guid>> HandleAsync(UpdateProfileCommand command, CancellationToken ct)
    {
        if (!userContext.IsAuthenticated || userContext.UserId is null)
        {
            return Result.Failure<Guid>(UserErrors.Unauthorized);
        }

        await identityService.UpdateProfileAsync(
            userContext.UserId.Value,
            command.PhoneNumber,
            command.FirstName,
            command.LastName,
            ct);

        return userContext.UserId.Value;
    }
}