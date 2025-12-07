namespace Presentation.Endpoints.Cinemas;

using Application.Contracts.Cinemas;
using Application.Queries.GetCinema;
using Extensions;
using Infrastructure;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class GetCinemaById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(CinemasRoutes.GetById, async (
                Guid id,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var query = new GetCinemaByIdQuery(id);

                var result = await mediator.ExecuteQueryAsync<GetCinemaByIdQuery, CinemaResponseModel>(query, ct);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .AllowAnonymous()
            .WithTags(Tags.Cinemas);
    }
}