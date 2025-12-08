namespace Presentation.Endpoints.Cinemas;

using Application.Commands.Cinemas.CreateCinema;
using Application.Contracts.Cinemas;
using Extensions;
using Infrastructure;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class CreateCinema : IEndpoint
{
    public sealed record CreateCinemaRequest(
        string Name,
        string City,
        string Address,
        double? Latitude,
        double? Longitude);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(CinemasRoutes.Create, async (
                CreateCinemaRequest request,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var command = new CreateCinemaCommand(
                    Name: request.Name,
                    City: request.City,
                    Address: request.Address,
                    Latitude: request.Latitude,
                    Longitude: request.Longitude);

                var result = await mediator.ExecuteCommandAsync<CreateCinemaCommand, CinemaResponseModel>(command, ct);

                return result.Match(
                    cinema => Results.Created(
                        CinemasRoutes.GetById.Replace("{id:guid}", cinema.Id.ToString()),
                        cinema),
                    CustomResults.Problem);
            })
            .RequireAuthorization("AdminPolicy")
            .WithTags(Tags.Cinemas);
    }
}