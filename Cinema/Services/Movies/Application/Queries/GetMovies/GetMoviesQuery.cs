namespace Application.Queries.GetMovies;

using Abstractions.Messaging;
using Contracts.Movies;
using Domain.Enums;

public sealed record GetMoviesQuery(
    Genres[]? Genres,
    int? MinDurationMinutes,
    int? MaxDurationMinutes,
    AgeRating? MinAgeRating,
    AgeRating? MaxAgeRating,
    Status? Status,
    int Page = 1,
    int PageSize = 20
) : IQuery<PagedMoviesResponseModel>;