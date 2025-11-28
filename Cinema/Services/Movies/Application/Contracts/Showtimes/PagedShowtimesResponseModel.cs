namespace Application.Contracts.Showtimes;

public sealed record PagedShowtimesResponseModel(
    int Page,
    int PageSize,
    int TotalCount,
    IReadOnlyCollection<ShowtimeResponseModel> Items);