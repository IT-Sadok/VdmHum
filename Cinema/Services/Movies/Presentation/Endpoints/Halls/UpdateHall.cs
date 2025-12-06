namespace Presentation.Endpoints.Halls;

using Application.Commands.Halls.UpdateHall;
using Extensions;
using Infrastructure;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class UpdateHall : IEndpoint
{
    public sealed record UpdateHallRequest(string Name, int NumberOfSeats);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(HallsRoutes.Update, async (
                Guid id,
                UpdateHallRequest request,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var command = new UpdateHallCommand(
                    Id: id,
                    Name: request.Name,
                    NumberOfSeats: request.NumberOfSeats);

                var result = await mediator.Send(command, ct);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .RequireAuthorization("AdminPolicy")
            .WithTags(Tags.Halls);
    }
}