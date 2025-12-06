namespace Application.Commands.CreateAdminUser;

using Contracts;
using Errors;
using Errors.Constants;
using Abstractions.Providers;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

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

        await using var transaction = await unitOfWork.BeginTransactionAsync(ct);

        var user = await identityService.RegisterAsync(
            command.Email,
            command.Password,
            command.PhoneNumber,
            command.FirstName,
            command.LastName,
            ct);

        await identityService.AddToRoleAsync(user, RoleNames.Admin, ct);

        await transaction.CommitAsync(ct);

        return new CreateAdminResponseModel(user.Id);
    }
}