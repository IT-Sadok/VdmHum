namespace Presentation.Endpoints.Halls;

using Application.Abstractions.Messaging;
using Application.Commands.Halls.UpdateHall;
using Application.Contracts.Halls;
using Extensions;
using Infrastructure;
using Routes;

internal sealed class UpdateHall : IEndpoint
{
    public sealed record UpdateHallRequest(string Name, int NumberOfSeats);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(HallsRoutes.Update, async (
                Guid id,
                UpdateHallRequest request,
                ICommandHandler<UpdateHallCommand, HallResponseModel> handler,
                CancellationToken ct) =>
            {
                var command = new UpdateHallCommand(
                    Id: id,
                    Name: request.Name,
                    NumberOfSeats: request.NumberOfSeats);

                var result = await handler.HandleAsync(command, ct);

                return result.Match(
                    hall => Results.Ok<HallResponseModel>(hall),
                    CustomResults.Problem);
            })
            .RequireAuthorization("AdminPolicy")
            .WithTags(Tags.Halls);
    }
}