namespace Presentation.Endpoints.Movies;

using Application.Contracts.Movies;
using Application.Queries.GetMovies;
using Domain.Enums;
using Extensions;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class GetMovies : IEndpoint
{
    public sealed record GetMoviesRequest(
        [FromQuery] Genres[]? Genres,
        [FromQuery] int? MinDurationMinutes,
        [FromQuery] int? MaxDurationMinutes,
        [FromQuery] AgeRating? MinAgeRating,
        [FromQuery] AgeRating? MaxAgeRating,
        [FromQuery] Status? Status,
        [FromQuery] int Page = 1,
        [FromQuery] int PageSize = 20);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(MoviesRoutes.GetPaged, async (
                [AsParameters] GetMoviesRequest request,
                IQueryHandler<GetMoviesQuery, PagedMoviesResponseModel> handler,
                CancellationToken ct) =>
            {
                var query = new GetMoviesQuery(
                    Genres: request.Genres,
                    MinDurationMinutes: request.MinDurationMinutes,
                    MaxDurationMinutes: request.MaxDurationMinutes,
                    MinAgeRating: request.MinAgeRating,
                    MaxAgeRating: request.MaxAgeRating,
                    Status: request.Status,
                    Page: request.Page,
                    PageSize: request.PageSize);

                var result = await handler.HandleAsync(query, ct);

                return result.Match(
                    paged => Results.Ok(paged),
                    CustomResults.Problem);
            })
            .AllowAnonymous()
            .WithTags(Tags.Movies);
    }
}