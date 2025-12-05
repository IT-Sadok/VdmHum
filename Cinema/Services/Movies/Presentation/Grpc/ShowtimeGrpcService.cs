namespace Presentation.Grpc;

using Application.Abstractions.Repositories;
using global::Grpc.Core;
using Google.Protobuf.WellKnownTypes;
using Showtime.Grpc;

public sealed class ShowtimeGrpcService(
    IShowtimeRepository showtimeRepo,
    IMovieRepository movieRepo,
    ICinemaRepository cinemaRepo,
    IHallRepository hallRepo)
    : ShowtimeService.ShowtimeServiceBase
{
    public override async Task<GetShowtimeResponse> GetShowtime(
        GetShowtimeRequest request,
        ServerCallContext context)
    {
        if (!Guid.TryParse(request.ShowtimeId, out var showtimeId))
        {
            return new GetShowtimeResponse { Found = false };
        }

        var showtime = await showtimeRepo.GetByIdAsync(showtimeId, asNoTracking: true, context.CancellationToken);

        if (showtime is null)
        {
            return new GetShowtimeResponse { Found = false };
        }

        var movie = await movieRepo.GetByIdAsync(showtime.MovieId, asNoTracking: true, context.CancellationToken);
        var cinema = await cinemaRepo.GetByIdAsync(showtime.CinemaId, asNoTracking: true, context.CancellationToken);
        var hall = await hallRepo.GetByIdAsync(showtime.HallId, asNoTracking: true, context.CancellationToken);

        if (movie is null || cinema is null || hall is null)
        {
            return new GetShowtimeResponse { Found = false };
        }

        return new GetShowtimeResponse
        {
            Found = true,
            Showtime = new ShowtimeModel
            {
                Id = showtime.Id.ToString(),
                MovieId = showtime.MovieId.ToString(),
                CinemaId = showtime.CinemaId.ToString(),
                HallId = showtime.HallId.ToString(),
                MovieTitle = movie.Title,
                CinemaName = cinema.Name,
                HallName = hall.Name,
                StartTimeUtc = Timestamp.FromDateTime(showtime.StartTimeUtc.ToUniversalTime()),
            },
        };
    }
}