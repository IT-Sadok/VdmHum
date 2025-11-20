namespace Application.Commands.CreateAdminUser;

using Domain;
using Domain.Abstractions;
using Domain.Constants;
using Abstractions.Providers;
using Abstractions.Messaging;

public class CreateAdminUserCommandHandler(
    IIdentityService identityService)
    : ICommandHandler<CreateAdminUserCommand, Guid>
{
    public async Task<Result<Guid>> Handle(CreateAdminUserCommand command, CancellationToken ct)
    {
        var existing = await identityService.FindByEmailAsync(command.Email, ct);

        if (existing is not null)
        {
            return Result.Failure<Guid>(UserErrors.EmailNotUnique);
        }

        var user = await identityService.RegisterAsync(
            command.Email,
            command.Password,
            command.PhoneNumber,
            command.FirstName,
            command.LastName,
            ct);

        await identityService.AddToRoleAsync(user, RoleNames.Admin, ct);

        return user.Id;
    }
}