namespace Application.Queries.GetCinemas;

using Abstractions.Messaging;
using Abstractions.Repositories;
using Contracts.Cinemas;
using Domain.Abstractions;
using Domain.Entities;

public sealed class GetCinemasQueryHandler(
    ICinemaRepository cinemaRepository)
    : IQueryHandler<GetCinemasQuery, PagedCinemasResponseModel>
{
    public async Task<Result<PagedCinemasResponseModel>> HandleAsync(
        GetCinemasQuery query,
        CancellationToken ct)
    {
        var filter = new CinemaFilter(
            Name: query.Name,
            City: query.City);

        var (items, totalCount) = await cinemaRepository.GetPagedAsync(
            filter,
            query.Page,
            query.PageSize,
            ct);

        var responseItems = items
            .Select(MapToResponse)
            .ToArray();

        var response = new PagedCinemasResponseModel(
            Page: query.Page,
            PageSize: query.PageSize,
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