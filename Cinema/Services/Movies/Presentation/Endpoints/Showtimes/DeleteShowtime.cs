namespace Presentation.Endpoints.Showtimes;

using Application.Commands.Showtimes.DeleteShowtime;
using Extensions;
using Infrastructure;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class DeleteShowtime : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(ShowtimesRoutes.Delete, async (
                Guid id,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var command = new DeleteShowtimeCommand(id);

                var result = await mediator.ExecuteCommandAsync(command, ct);

                return result.Match(
                    Results.NoContent,
                    CustomResults.Problem);
            })
            .RequireAuthorization("AdminPolicy")
            .WithTags(Tags.Showtimes);
    }
}