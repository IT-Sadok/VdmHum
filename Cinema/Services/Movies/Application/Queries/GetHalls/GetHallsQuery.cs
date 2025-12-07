namespace Application.Queries.GetHalls;

using Contracts;
using Shared.Contracts.Abstractions;
using Contracts.Halls;

public sealed record GetHallsQuery(
    PagedFilter<HallFilter> PagedFilter
) : IQuery<PagedResponse<HallResponseModel>>;