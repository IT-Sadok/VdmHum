namespace Application.Commands.CreateAdminUser;

using Contracts;
using Domain;
using Domain.Abstractions;
using Domain.Constants;
using Abstractions.Providers;
using Abstractions.Messaging;

public class CreateAdminUserCommandHandler(
    IIdentityService identityService,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateAdminUserCommand, CreateAdminResponseModel>
{
    public async Task<Result<CreateAdminResponseModel>> HandleAsync(
        CreateAdminUserCommand command,
        CancellationToken ct)
    {
        var existing = await identityService.FindByEmailAsync(command.Email, ct);

        if (existing is not null)
        {
            return Result.Failure<CreateAdminResponseModel>(UserErrors.EmailNotUnique);
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

                await identityService.AddToRoleAsync(user, RoleNames.Admin, innerCt);

                return new CreateAdminResponseModel(user.Id);
            },
            ct);
    }
}