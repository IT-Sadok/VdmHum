namespace Presentation.Endpoints.Cinemas;

using Application.Abstractions.Messaging;
using Application.Commands.Cinemas.DeleteCinema;
using Extensions;
using Infrastructure;
using Routes;

internal sealed class DeleteCinema : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(CinemasRoutes.Delete, async (
                Guid id,
                ICommandHandler<DeleteCinemaCommand> handler,
                CancellationToken ct) =>
            {
                var command = new DeleteCinemaCommand(id);

                var result = await handler.HandleAsync(command, ct);

                return result.Match(
                    Results.NoContent,
                    CustomResults.Problem);
            })
            .RequireAuthorization("AdminPolicy")
            .WithTags(Tags.Cinemas);
    }
}