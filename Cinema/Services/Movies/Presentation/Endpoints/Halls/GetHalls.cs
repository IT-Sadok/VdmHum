namespace Presentation.Endpoints.Halls;

using Application.Contracts.Halls;
using Application.Queries.GetHalls;
using Extensions;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class GetHalls : IEndpoint
{
    public sealed record GetHallsRequest(
        [FromQuery] Guid? CinemaId,
        [FromQuery] string? Name,
        [FromQuery] int Page = 1,
        [FromQuery] int PageSize = 20);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(HallsRoutes.GetPaged, async (
                [AsParameters] GetHallsRequest request,
                IQueryHandler<GetHallsQuery, PagedHallsResponseModel> handler,
                CancellationToken ct) =>
            {
                var query = new GetHallsQuery(
                    CinemaId: request.CinemaId,
                    Name: request.Name,
                    Page: request.Page,
                    PageSize: request.PageSize);

                var result = await handler.HandleAsync(query, ct);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .AllowAnonymous()
            .WithTags(Tags.Halls);
    }
}