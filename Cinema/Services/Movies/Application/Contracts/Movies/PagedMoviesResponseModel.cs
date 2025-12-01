namespace Application.Contracts.Movies;

public sealed record PagedMoviesResponseModel(
    int Page,
    int PageSize,
    int TotalCount,
    IReadOnlyCollection<MovieResponseModel> Items);