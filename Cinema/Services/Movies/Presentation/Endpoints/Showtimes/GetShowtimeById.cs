namespace Presentation.Endpoints.Showtimes;

using Application.Queries.GetShowtime;
using Extensions;
using Infrastructure;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class GetShowtimeById : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapGet(ShowtimesRoutes.GetById, async (
                Guid id,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var query = new GetShowtimeByIdQuery(id);

                var result = await mediator.Send(query, ct);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .AllowAnonymous()
            .WithTags(Tags.Showtimes);
    }
}