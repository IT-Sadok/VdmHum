namespace Application.Queries.GetShowtime;

using Abstractions.Messaging;
using Abstractions.Repositories;
using Contracts.Showtimes;
using Domain.Abstractions;
using Domain.Errors;

public sealed class GetShowtimeByIdQueryHandler(
    IShowtimeRepository showtimeRepository)
    : IQueryHandler<GetShowtimeByIdQuery, ShowtimeResponseModel>
{
    public async Task<Result<ShowtimeResponseModel>> HandleAsync(
        GetShowtimeByIdQuery query,
        CancellationToken ct)
    {
        var showtime = await showtimeRepository.GetByIdAsync(query.Id, true, ct);

        if (showtime is null)
        {
            return Result.Failure<ShowtimeResponseModel>(ShowtimeErrors.NotFound(query.Id));
        }

        var response = new ShowtimeResponseModel(
            showtime.Id,
            showtime.MovieId,
            showtime.CinemaId,
            showtime.HallId,
            showtime.StartTimeUtc,
            showtime.EndTimeUtc,
            showtime.BasePrice,
            showtime.Currency,
            showtime.Status,
            showtime.Language,
            showtime.Format,
            showtime.CancelReason);

        return response;
    }
}