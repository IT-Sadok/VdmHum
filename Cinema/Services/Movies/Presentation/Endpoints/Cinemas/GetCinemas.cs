namespace Presentation.Endpoints.Cinemas;

using Application.Contracts;
using Application.Contracts.Cinemas;
using Application.Queries.GetCinemas;
using Extensions;
using ErrorHandling;
using Microsoft.AspNetCore.Mvc;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class GetCinemas : IEndpoint
{
    public sealed record GetCinemasRequest(
        [FromQuery] string? Name,
        [FromQuery] string? City,
        [FromQuery] int Page = 1,
        [FromQuery] int PageSize = 20);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(CinemasRoutes.GetPaged, async (
                [AsParameters] GetCinemasRequest request,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var query = new GetCinemasQuery(
                    new PagedFilter<CinemaFilter>(
                        ModelFilter: new CinemaFilter(
                            Name: request.Name,
                            City: request.City),
                        Page: request.Page,
                        PageSize: request.PageSize));

                var result = await mediator.ExecuteQueryAsync
                    <GetCinemasQuery, PagedResponse<CinemaResponseModel>>(query, ct);

                return result.Match(
                    paged => Results.Ok(paged),
                    CustomResults.Problem);
            })
            .AllowAnonymous()
            .WithTags(Tags.Cinemas);
    }
}