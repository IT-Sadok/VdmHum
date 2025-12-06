namespace Application.Commands.Movies.UpdateMovie;

using Contracts.Movies;
using Domain.Enums;
using Shared.Contracts.Abstractions;

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