namespace Application.Queries.GetCurrentUser;

using Errors;
using Abstractions.Providers;
using Contracts;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public class GetCurrentUserHandler(
    IIdentityService identityService,
    IUserContext userContext
)
    : IQueryHandler<GetCurrentUserQuery, UserResponseModel>
{
    public async Task<Result<UserResponseModel>> HandleAsync(GetCurrentUserQuery query, CancellationToken ct)
    {
        if (!userContext.IsAuthenticated || userContext.UserId is null)
        {
            return Result.Failure<UserResponseModel>(UserErrors.Unauthorized);
        }

        var user = await identityService.GetByIdAsync(userContext.UserId.Value, ct);

        if (user is null)
        {
            return Result.Failure<UserResponseModel>(UserErrors.NotFound(userContext.UserId.Value));
        }

        return new UserResponseModel(
            user.Id,
            user.Email,
            user.PhoneNumber,
            user.FirstName,
            user.LastName,
            user.Roles);
    }
}