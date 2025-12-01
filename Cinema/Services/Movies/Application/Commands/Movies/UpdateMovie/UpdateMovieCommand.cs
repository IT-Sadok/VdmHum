namespace Application.Commands.Movies.UpdateMovie;

using Abstractions.Messaging;
using Contracts.Movies;
using Domain.Enums;

public sealed record UpdateMovieCommand(
    Guid Id,
    string Title,
    Status Status,
    string? Description,
    IReadOnlyCollection<Genres>? Genres,
    int? DurationMinutes,
    AgeRating? AgeRating,
    DateOnly? ReleaseDate,
    string? PosterUrl
) : ICommand<MovieResponseModel>;