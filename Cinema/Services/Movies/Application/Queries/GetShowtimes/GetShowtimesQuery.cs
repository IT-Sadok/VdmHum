namespace Application.Queries.GetShowtimes;

using Contracts;
using Shared.Contracts.Abstractions;
using Contracts.Showtimes;

public sealed record GetShowtimesQuery(
    PagedFilter<ShowtimeFilter> PagedFilter
) : IQuery<PagedResponse<ShowtimeResponseModel>>;