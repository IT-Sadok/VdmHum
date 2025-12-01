namespace Application.Queries.GetShowtimes;

using Abstractions.Messaging;
using Abstractions.Repositories;
using Contracts.Showtimes;
using Domain.Abstractions;
using Domain.Entities;

public sealed class GetShowtimesQueryHandler(
    IShowtimeRepository showtimeRepository)
    : IQueryHandler<GetShowtimesQuery, PagedShowtimesResponseModel>
{
    public async Task<Result<PagedShowtimesResponseModel>> HandleAsync(
        GetShowtimesQuery query,
        CancellationToken ct)
    {
        var filter = new ShowtimeFilter(
            MovieId: query.MovieId,
            CinemaId: query.CinemaId,
            HallId: query.HallId,
            DateFromUtc: query.DateFromUtc,
            DateToUtc: query.DateToUtc,
            Status: query.Status);

        var (items, totalCount) = await showtimeRepository.GetPagedAsync(
            filter,
            query.Page,
            query.PageSize,
            ct);

        var responseItems = items
            .Select(MapToResponse)
            .ToArray();

        var response = new PagedShowtimesResponseModel(
            Page: query.Page,
            PageSize: query.PageSize,
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