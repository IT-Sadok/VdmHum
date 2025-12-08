namespace Application.Queries.GetCinemas;

using Abstractions.Repositories;
using Contracts;
using Contracts.Cinemas;
using Domain.Entities;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class GetCinemasQueryHandler(
    ICinemaRepository cinemaRepository)
    : IQueryHandler<GetCinemasQuery, PagedResponse<CinemaResponseModel>>
{
    public async Task<Result<PagedResponse<CinemaResponseModel>>> HandleAsync(
        GetCinemasQuery query,
        CancellationToken ct)
    {
        var (items, totalCount) = await cinemaRepository.GetPagedAsync(query.PagedFilter, ct);

        var responseItems = items
            .Select(MapToResponse)
            .ToArray();

        var response = new PagedResponse<CinemaResponseModel>(
            Page: query.PagedFilter.Page,
            PageSize: query.PagedFilter.PageSize,
            TotalCount: totalCount,
            Items: responseItems);

        return response;
    }

    private static CinemaResponseModel MapToResponse(Cinema cinema) =>
        new(
            cinema.Id,
            cinema.Name,
            cinema.City,
            cinema.Address,
            cinema.Latitude,
            cinema.Longitude);
}