namespace Application.Contracts.Movies;

using Domain.Enums;

public sealed record MovieResponseModel(
    Guid Id,
    string Title,
    string? Description,
    IReadOnlyCollection<Genres> Genres,
    int? DurationMinutes,
    AgeRating? AgeRating,
    Status Status,
    DateOnly? ReleaseDate,
    string? PosterUrl);