namespace Presentation.Endpoints.Movies;

using Application.Commands.Movies.UpdateMovie;
using Application.Contracts.Movies;
using Domain.Enums;
using Extensions;
using Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class UpdateMovie : IEndpoint
{
    public sealed record UpdateMovieRequest(
        string Title,
        Status Status,
        string? Description,
        Genres[]? Genres,
        int? DurationMinutes,
        AgeRating? AgeRating,
        DateOnly? ReleaseDate,
        string? PosterUrl);

    public void MapEndpoint(IEndpointRouteBuilder app)
    {
        app.MapPut(MoviesRoutes.Update, async (
                [FromRoute] Guid id,
                UpdateMovieRequest request,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var command = new UpdateMovieCommand(
                    Id: id,
                    Title: request.Title,
                    Status: request.Status,
                    Description: request.Description,
                    Genres: request.Genres,
                    DurationMinutes: request.DurationMinutes,
                    AgeRating: request.AgeRating,
                    ReleaseDate: request.ReleaseDate,
                    PosterUrl: request.PosterUrl);

                var result = await mediator.ExecuteCommandAsync<UpdateMovieCommand, MovieResponseModel>(command, ct);

                return result.Match(
                    Results.Ok,
                    CustomResults.Problem);
            })
            .RequireAuthorization("AdminPolicy")
            .WithTags(Tags.Movies);
    }
}