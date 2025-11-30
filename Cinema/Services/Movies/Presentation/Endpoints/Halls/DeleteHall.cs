namespace Presentation.Endpoints.Halls;

using Application.Abstractions.Messaging;
using Application.Commands.Halls.DeleteHall;
using Extensions;
using Infrastructure;
using Routes;

internal sealed class DeleteHall : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(HallsRoutes.Delete, async (
                Guid id,
                ICommandHandler<DeleteHallCommand> handler,
                CancellationToken ct) =>
            {
                var command = new DeleteHallCommand(id);

                var result = await handler.HandleAsync(command, ct);

                return result.Match(
                    Results.NoContent,
                    CustomResults.Problem);
            })
            .RequireAuthorization("AdminPolicy")
            .WithTags(Tags.Halls);
    }
}