namespace Presentation.Endpoints.Users;

using Application.Contracts;
using Routes;
using Application.Queries.GetCurrentUser;
using Extensions;
using ErrorHandling;
using Shared.Contracts.Abstractions;

internal sealed class GetCurrent : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(UsersRoutes.GetCurrent, async (
                IMediator mediator,
                CancellationToken ct) =>
            {
                var query = new GetCurrentUserQuery();

                var result = await mediator.ExecuteQueryAsync<GetCurrentUserQuery, UserResponseModel>(query, ct);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Users);
    }
}