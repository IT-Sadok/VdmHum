namespace Presentation.Endpoints.Users;

using Routes;
using Application.Contracts;
using Application.Queries.GetCurrentUser;
using Extensions;
using Infrastructure;
using Shared.Contracts.Abstractions;

internal sealed class GetCurrent : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(UsersRoutes.GetCurrent, async (
                IQueryHandler<GetCurrentUserQuery, UserResponseModel> handler,
                CancellationToken ct) =>
            {
                var query = new GetCurrentUserQuery();

                var result = await handler.HandleAsync(query, ct);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Users);
    }
}