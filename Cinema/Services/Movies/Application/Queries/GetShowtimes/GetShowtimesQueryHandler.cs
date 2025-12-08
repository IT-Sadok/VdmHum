namespace Application.Queries.GetShowtimes;

using Abstractions.Repositories;
using Contracts;
using Contracts.Showtimes;
using Domain.Entities;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class GetShowtimesQueryHandler(
    IShowtimeRepository showtimeRepository)
    : IQueryHandler<GetShowtimesQuery, PagedResponse<ShowtimeResponseModel>>
{
    public async Task<Result<PagedResponse<ShowtimeResponseModel>>> HandleAsync(
        GetShowtimesQuery query,
        CancellationToken ct)
    {
        var (items, totalCount) = await showtimeRepository.GetPagedAsync(
            query.PagedFilter,
            ct);

        var responseItems = items
            .Select(MapToResponse)
            .ToArray();

        var response = new PagedResponse<ShowtimeResponseModel>(
            Page: query.PagedFilter.Page,
            PageSize: query.PagedFilter.PageSize,
            TotalCount: totalCount,
            Items: responseItems);

        return response;
    }

    private static ShowtimeResponseModel MapToResponse(Showtime s) =>
        new(
            s.Id,
            s.MovieId,
            s.CinemaId,
            s.HallId,
            s.StartTimeUtc,
            s.EndTimeUtc,
            s.BasePrice,
            s.Currency,
            s.Status,
            s.Language,
            s.Format,
            s.CancelReason);
}