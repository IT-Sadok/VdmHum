namespace Presentation.Endpoints.Showtimes;

using Application.Abstractions.Messaging;
using Application.Commands.Showtimes.DeleteShowtime;
using Extensions;
using Infrastructure;
using Routes;

internal sealed class DeleteShowtime : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(ShowtimesRoutes.Delete, async (
                Guid id,
                ICommandHandler<DeleteShowtimeCommand> handler,
                CancellationToken ct) =>
            {
                var command = new DeleteShowtimeCommand(id);

                var result = await handler.HandleAsync(command, ct);

                return result.Match(
                    Results.NoContent,
                    CustomResults.Problem);
            })
            .RequireAuthorization("AdminPolicy")
            .WithTags(Tags.Showtimes);
    }
}