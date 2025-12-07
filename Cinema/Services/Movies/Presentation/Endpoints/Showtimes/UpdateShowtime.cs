namespace Presentation.Endpoints.Showtimes;

using Application.Commands.Showtimes.UpdateShowtime;
using Application.Contracts.Showtimes;
using Domain.Enums;
using Extensions;
using Infrastructure;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class UpdateShowtime : IEndpoint
{
    public sealed record UpdateShowtimeRequest(
        DateTime StartTimeUtc,
        DateTime EndTimeUtc,
        decimal BasePrice,
        ShowtimeStatus Status,
        string? Language,
        string? Format,
        string? CancelReason);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(ShowtimesRoutes.Update, async (
                Guid id,
                UpdateShowtimeRequest request,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var command = new UpdateShowtimeCommand(
                    Id: id,
                    StartTimeUtc: request.StartTimeUtc,
                    EndTimeUtc: request.EndTimeUtc,
                    BasePrice: request.BasePrice,
                    Status: request.Status,
                    Language: request.Language,
                    Format: request.Format,
                    CancelReason: request.CancelReason);

                var result = await mediator.ExecuteCommandAsync<UpdateShowtimeCommand, ShowtimeResponseModel>(command, ct);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .RequireAuthorization("AdminPolicy")
            .WithTags(Tags.Showtimes);
    }
}