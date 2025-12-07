namespace Presentation.Endpoints.Halls;

using Application.Contracts.Halls;
using Application.Queries.GetHall;
using Extensions;
using Infrastructure;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class GetHallById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(HallsRoutes.GetById, async (
                Guid id,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var query = new GetHallByIdQuery(id);

                var result = await mediator.ExecuteQueryAsync<GetHallByIdQuery, HallResponseModel>(query, ct);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .AllowAnonymous()
            .WithTags(Tags.Halls);
    }
}