namespace Presentation.Endpoints.Halls;

using Application.Commands.Halls.CreateHall;
using Application.Contracts.Halls;
using Extensions;
using Infrastructure;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class CreateHall : IEndpoint
{
    public sealed record CreateHallRequest(
        Guid CinemaId,
        string Name,
        int NumberOfSeats);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(HallsRoutes.Create, async (
                CreateHallRequest request,
                ICommandHandler<CreateHallCommand, HallResponseModel> handler,
                CancellationToken ct) =>
            {
                var command = new CreateHallCommand(
                    CinemaId: request.CinemaId,
                    Name: request.Name,
                    NumberOfSeats: request.NumberOfSeats);

                var result = await handler.HandleAsync(command, ct);

                return result.Match(
                    hall => Results.Created(
                        HallsRoutes.GetById.Replace("{id:guid}", hall.Id.ToString()),
                        hall),
                    CustomResults.Problem);
            })
            .RequireAuthorization("AdminPolicy")
            .WithTags(Tags.Halls);
    }
}