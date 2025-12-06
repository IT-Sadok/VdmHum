namespace Presentation.Endpoints.Halls;

using Application.Commands.Halls.DeleteHall;
using Extensions;
using Infrastructure;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class DeleteHall : IEndpoint
{
    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapDelete(HallsRoutes.Delete, async (
                Guid id,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var command = new DeleteHallCommand(id);

                var result = await mediator.Send(command, ct);

                return result.Match(
                    Results.NoContent,
                    CustomResults.Problem);
            })
            .RequireAuthorization("AdminPolicy")
            .WithTags(Tags.Halls);
    }
}