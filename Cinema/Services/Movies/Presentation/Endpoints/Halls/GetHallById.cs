namespace Presentation.Endpoints.Halls;

using Application.Abstractions.Messaging;
using Application.Contracts.Halls;
using Application.Queries.GetHall;
using Extensions;
using Infrastructure;
using Routes;

internal sealed class GetHallById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(HallsRoutes.GetById, async (
                Guid id,
                IQueryHandler<GetHallByIdQuery, HallResponseModel> handler,
                CancellationToken ct) =>
            {
                var query = new GetHallByIdQuery(id);

                var result = await handler.HandleAsync(query, ct);

                return result.Match(
                    Results.Ok<HallResponseModel>,
                    CustomResults.Problem);
            })
            .AllowAnonymous()
            .WithTags(Tags.Halls);
    }
}