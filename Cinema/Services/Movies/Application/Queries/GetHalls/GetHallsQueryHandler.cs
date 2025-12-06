namespace Application.Queries.GetHalls;

using Abstractions.Repositories;
using Contracts.Halls;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class GetHallsQueryHandler(
    IHallRepository hallRepository)
    : IQueryHandler<GetHallsQuery, PagedHallsResponseModel>
{
    public async Task<Result<PagedHallsResponseModel>> HandleAsync(
        GetHallsQuery query,
        CancellationToken ct)
    {
        var (items, totalCount) = await hallRepository.GetPagedAsync(
            query.CinemaId,
            query.Name,
            query.Page,
            query.PageSize,
            ct);

        var responseItems = items
            .Select(h => new HallResponseModel(
                h.Id,
                h.CinemaId,
                h.Name,
                h.NumberOfSeats))
            .ToArray();

        var response = new PagedHallsResponseModel(
            Page: query.Page,
            PageSize: query.PageSize,
            TotalCount: totalCount,
            Items: responseItems);

        return response;
    }
}