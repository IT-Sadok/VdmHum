namespace Application.Commands.UpdateProfile;

using Domain;
using Domain.Abstractions;
using Abstractions.Providers;
using Abstractions.Messaging;

public class UpdateProfileCommandHandler(
    IIdentityService identityService,
    ICurrentUserService currentUserService)
    : ICommandHandler<UpdateProfileCommand, Guid>
{
    public async Task<Result<Guid>> Handle(UpdateProfileCommand command, CancellationToken ct)
    {
        if (!currentUserService.IsAuthenticated || currentUserService.UserId is null)
        {
            return Result.Failure<Guid>(UserErrors.Unauthorized);
        }

        await identityService.UpdateProfileAsync(
            currentUserService.UserId.Value,
            command.PhoneNumber,
            command.FirstName,
            command.LastName,
            ct);

        return currentUserService.UserId.Value;
    }
}