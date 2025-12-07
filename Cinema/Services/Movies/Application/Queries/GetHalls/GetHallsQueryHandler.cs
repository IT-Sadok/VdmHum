namespace Application.Queries.GetHalls;

using Abstractions.Repositories;
using Contracts;
using Contracts.Halls;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class GetHallsQueryHandler(
    IHallRepository hallRepository)
    : IQueryHandler<GetHallsQuery, PagedResponse<HallResponseModel>>
{
    public async Task<Result<PagedResponse<HallResponseModel>>> HandleAsync(
        GetHallsQuery query,
        CancellationToken ct)
    {
        var (items, totalCount) = await hallRepository.GetPagedAsync(
            query.PagedFilter,
            ct);

        var responseItems = items
            .Select(h => new HallResponseModel(
                h.Id,
                h.CinemaId,
                h.Name,
                h.NumberOfSeats))
            .ToArray();

        var response = new PagedResponse<HallResponseModel>(
            Page: query.PagedFilter.Page,
            PageSize: query.PagedFilter.PageSize,
            TotalCount: totalCount,
            Items: responseItems);

        return response;
    }
}