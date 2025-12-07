namespace Application.Queries.GetCinemas;

using Contracts;
using Shared.Contracts.Abstractions;
using Contracts.Cinemas;

public sealed record GetCinemasQuery(
    PagedFilter<CinemaFilter> PagedFilter
) : IQuery<PagedResponse<CinemaResponseModel>>;