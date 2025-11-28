namespace Application.Contracts.Cinemas;

public sealed record PagedCinemasResponseModel(
    int Page,
    int PageSize,
    int TotalCount,
    IReadOnlyCollection<CinemaResponseModel> Items);