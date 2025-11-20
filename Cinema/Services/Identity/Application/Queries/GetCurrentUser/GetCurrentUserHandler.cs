namespace Application.Queries.GetCurrentUser;

using Domain;
using Abstractions.Providers;
using Abstractions.Messaging;
using Contracts;
using Domain.Abstractions;

public class GetCurrentUserHandler(
    IIdentityService identityService,
    ICurrentUserService currentUserService
)
    : IQueryHandler<GetCurrentUserQuery, UserResponseModel>
{
    public async Task<Result<UserResponseModel>> Handle(GetCurrentUserQuery query, CancellationToken ct)
    {
        if (!currentUserService.IsAuthenticated || currentUserService.UserId is null)
        {
            return Result.Failure<UserResponseModel>(UserErrors.Unauthorized);
        }

        var user = await identityService.GetByIdAsync(currentUserService.UserId.Value, ct);

        if (user is null)
        {
            return Result.Failure<UserResponseModel>(UserErrors.NotFound(currentUserService.UserId.Value));
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