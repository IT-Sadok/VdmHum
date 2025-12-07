namespace Presentation.Endpoints.Movies;

using Application.Contracts.Movies;
using Application.Queries.GetMovie;
using Extensions;
using Infrastructure;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class GetMovieById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(MoviesRoutes.GetById, async (
                Guid id,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var query = new GetMovieByIdQuery(id);

                var result = await mediator.ExecuteQueryAsync<GetMovieByIdQuery, MovieResponseModel>(query, ct);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .AllowAnonymous()
            .WithTags(Tags.Movies);
    }
}