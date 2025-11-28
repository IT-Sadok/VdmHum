namespace Application.Contracts.Halls;

public sealed record PagedHallsResponseModel(
    int Page,
    int PageSize,
    int TotalCount,
    IReadOnlyCollection<HallResponseModel> Items);