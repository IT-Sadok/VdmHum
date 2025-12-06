namespace Application.Commands.Movies.CreateMovie;

using Application.Contracts.Movies;
using Domain.Enums;
using Shared.Contracts.Abstractions;

public sealed record CreateMovieCommand(
    string Title,
    Status Status,
    string? Description,
    IReadOnlyCollection<Genres>? Genres,
    int? DurationMinutes,
    AgeRating? AgeRating,
    DateOnly? ReleaseDate,
    string? PosterUrl
) : ICommand<MovieResponseModel>;