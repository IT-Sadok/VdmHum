namespace Infrastructure.Services;

using Application.Abstractions.Services;
using Domain.ValueObjects;
using Showtime.Grpc;

public sealed class ShowtimeReadService(ShowtimeService.ShowtimeServiceClient client)
    : IShowtimeReadService
{
    public async Task<ShowtimeSnapshot?> GetShowtimeSnapshotAsync(
        Guid showtimeId,
        CancellationToken ct)
    {
        var response = await client.GetShowtimeAsync(
            new GetShowtimeRequest { ShowtimeId = showtimeId.ToString() },
            cancellationToken: ct);

        if (!response.Found || response.Showtime is null)
        {
            return null;
        }

        var showtimeDto = response.Showtime;

        return new ShowtimeSnapshot(
            ShowtimeId: Guid.Parse(showtimeDto.Id),
            MovieId: Guid.Parse(showtimeDto.MovieId),
            CinemaId: Guid.Parse(showtimeDto.CinemaId),
            HallId: Guid.Parse(showtimeDto.HallId),
            MovieTitle: showtimeDto.MovieTitle,
            CinemaName: showtimeDto.CinemaName,
            HallName: showtimeDto.HallName,
            StartTimeUtc: showtimeDto.StartTimeUtc.ToDateTime().ToUniversalTime());
    }
}