namespace Presentation.Endpoints.Users;

using Application.Abstractions.Messaging;
using Application.Contracts;
using Application.Queries.GetCurrentUser;
using Extensions;
using Infrastructure;

internal sealed class GetCurrent : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet("users/me", async (
                IQueryHandler<GetCurrentUserQuery, UserResponseModel> handler,
                CancellationToken ct) =>
            {
                var query = new GetCurrentUserQuery();

                var result = await handler.Handle(query, ct);

                return result.Match(Results.Ok, CustomResults.Problem);
            })
            .RequireAuthorization()
            .WithTags(Tags.Users);
    }
}