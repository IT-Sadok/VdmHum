namespace Presentation.Endpoints.Cinemas;

using Application.Commands.Cinemas.UpdateCinema;
using Application.Contracts.Cinemas;
using Extensions;
using Infrastructure;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class UpdateCinema : IEndpoint
{
    public sealed record UpdateCinemaRequest(
        string Name,
        string City,
        string Address,
        double? Latitude,
        double? Longitude);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(CinemasRoutes.Update, async (
                Guid id,
                UpdateCinemaRequest request,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var command = new UpdateCinemaCommand(
                    Id: id,
                    Name: request.Name,
                    City: request.City,
                    Address: request.Address,
                    Latitude: request.Latitude,
                    Longitude: request.Longitude);

                var result = await mediator.ExecuteCommandAsync<UpdateCinemaCommand, CinemaResponseModel>(command, ct);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .RequireAuthorization("AdminPolicy")
            .WithTags(Tags.Cinemas);
    }
}