namespace Presentation.Endpoints.Showtimes;

using Application.Commands.Showtimes.CreateShowtime;
using Domain.Enums;
using Extensions;
using Infrastructure;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class CreateShowtime : IEndpoint
{
    public sealed record CreateShowtimeRequest(
        Guid MovieId,
        Guid CinemaId,
        Guid HallId,
        DateTime StartTimeUtc,
        DateTime EndTimeUtc,
        decimal BasePrice,
        string Currency,
        ShowtimeStatus Status = ShowtimeStatus.Scheduled,
        string? Language = null,
        string? Format = null);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPost(ShowtimesRoutes.Create, async (
                CreateShowtimeRequest request,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var command = new CreateShowtimeCommand(
                    MovieId: request.MovieId,
                    CinemaId: request.CinemaId,
                    HallId: request.HallId,
                    StartTimeUtc: request.StartTimeUtc,
                    EndTimeUtc: request.EndTimeUtc,
                    BasePrice: request.BasePrice,
                    Currency: request.Currency,
                    Status: request.Status,
                    Language: request.Language,
                    Format: request.Format);

                var result = await mediator.Send(command, ct);

                return result.Match(
                    showtime => Results.Created(
                        ShowtimesRoutes.GetById.Replace("{id:guid}", showtime.Id.ToString()),
                        showtime),
                    CustomResults.Problem);
            })
            .RequireAuthorization("AdminPolicy")
            .WithTags(Tags.Showtimes);
    }
}