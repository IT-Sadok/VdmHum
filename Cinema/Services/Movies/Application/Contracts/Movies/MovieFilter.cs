namespace Application.Contracts.Movies;

using Domain.Enums;

public sealed record MovieFilter(
    IReadOnlyCollection<Genres>? Genres,
    int? MinDurationMinutes,
    int? MaxDurationMinutes,
    AgeRating? MinAgeRating,
    AgeRating? MaxAgeRating,
    Status? Status);