namespace Presentation.Endpoints.Movies;

using Application.Commands.Movies.CreateMovie;
using Domain.Enums;
using Extensions;
using Infrastructure;
using Routes;
using Shared.Contracts.Abstractions;

internal sealed class CreateMovie : IEndpoint
{
    public sealed record CreateMovieRequest(
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
        app.MapPost(MoviesRoutes.Create, async (
                CreateMovieRequest request,
                IMediator mediator,
                CancellationToken ct) =>
            {
                var command = new CreateMovieCommand(
                    Title: request.Title,
                    Status: request.Status,
                    Description: request.Description,
                    Genres: request.Genres,
                    DurationMinutes: request.DurationMinutes,
                    AgeRating: request.AgeRating,
                    ReleaseDate: request.ReleaseDate,
                    PosterUrl: request.PosterUrl);

                var result = await mediator.Send(command, ct);

                return result.Match(
                    movie => Results.Created(
                        MoviesRoutes.GetById.Replace("{id:guid}", movie.Id.ToString()),
                        movie),
                    CustomResults.Problem);
            })
            .RequireAuthorization("AdminPolicy")
            .WithTags(Tags.Movies);
    }
}