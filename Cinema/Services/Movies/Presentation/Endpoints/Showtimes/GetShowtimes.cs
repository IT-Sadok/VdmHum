namespace Presentation.Endpoints.Showtimes;

using Application.Contracts;
using Application.Contracts.Showtimes;
using Application.Queries.GetShowtimes;
using Domain.Enums;
using Extensions;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class GetShowtimes : IEndpoint
{
    public sealed record GetShowtimesRequest(
        [FromQuery] Guid? MovieId,
        [FromQuery] Guid? CinemaId,
        [FromQuery] Guid? HallId,
        [FromQuery] DateTime? DateFromUtc,
        [FromQuery] DateTime? DateToUtc,
        [FromQuery] ShowtimeStatus? Status,
        [FromQuery] int Page = 1,
        [FromQuery] int PageSize = 20);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ShowtimesRoutes.GetPaged, async (
                [AsParameters] GetShowtimesRequest request,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var query = new GetShowtimesQuery(
                    new PagedFilter<ShowtimeFilter>(
                        new ShowtimeFilter(
                            MovieId: request.MovieId,
                            CinemaId: request.CinemaId,
                            HallId: request.HallId,
                            DateFromUtc: request.DateFromUtc,
                            DateToUtc: request.DateToUtc,
                            Status: request.Status),
                        Page: request.Page,
                        PageSize: request.PageSize));

                var result =
                    await mediator.ExecuteQueryAsync
                        <GetShowtimesQuery, PagedResponse<ShowtimeResponseModel>>(query, ct);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .AllowAnonymous()
            .WithTags(Tags.Showtimes);
    }
}