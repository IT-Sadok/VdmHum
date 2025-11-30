namespace Presentation.Endpoints.Movies;

using Application.Abstractions.Messaging;
using Application.Commands.Movies.UpdateMovie;
using Application.Contracts.Movies;
using Domain.Enums;
using Extensions;
using Infrastructure;
using Routes;

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
                Guid id,
                UpdateMovieRequest request,
                ICommandHandler<UpdateMovieCommand, MovieResponseModel> handler,
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

                var result = await handler.HandleAsync(command, ct);

                return result.Match(
                    Results.Ok<MovieResponseModel>,
                    CustomResults.Problem);
            })
            .RequireAuthorization("AdminPolicy")
            .WithTags(Tags.Movies);
    }
}