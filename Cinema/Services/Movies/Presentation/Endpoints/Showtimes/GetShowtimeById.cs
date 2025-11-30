namespace Presentation.Endpoints.Showtimes;

using Application.Abstractions.Messaging;
using Application.Contracts.Showtimes;
using Application.Queries.GetShowtime;
using Extensions;
using Infrastructure;
using Routes;

internal sealed class GetShowtimeById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ShowtimesRoutes.GetById, async (
                Guid id,
                IQueryHandler<GetShowtimeByIdQuery, ShowtimeResponseModel> handler,
                CancellationToken ct) =>
            {
                var query = new GetShowtimeByIdQuery(id);

                var result = await handler.HandleAsync(query, ct);

                return result.Match(
                    Results.Ok<ShowtimeResponseModel>,
                    CustomResults.Problem);
            })
            .AllowAnonymous()
            .WithTags(Tags.Showtimes);
    }
}