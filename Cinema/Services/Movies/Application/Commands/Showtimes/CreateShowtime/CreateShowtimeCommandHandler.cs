namespace Application.Commands.Showtimes.CreateShowtime;

using Abstractions.Messaging;
using Abstractions.Repositories;
using Contracts.Showtimes;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;

public sealed class CreateShowtimeCommandHandler(
    IShowtimeRepository showtimeRepository,
    IMovieRepository movieRepository,
    ICinemaRepository cinemaRepository,
    IHallRepository hallRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateShowtimeCommand, ShowtimeResponseModel>
{
    public async Task<Result<ShowtimeResponseModel>> HandleAsync(
        CreateShowtimeCommand command,
        CancellationToken ct)
    {
        var movie = await movieRepository.GetByIdAsync(command.MovieId, true, ct);
        if (movie is null)
        {
            return Result.Failure<ShowtimeResponseModel>(ShowtimeErrors.MovieNotFound(command.MovieId));
        }

        var cinema = await cinemaRepository.GetByIdAsync(command.CinemaId, true, ct);
        if (cinema is null)
        {
            return Result.Failure<ShowtimeResponseModel>(ShowtimeErrors.CinemaNotFound(command.CinemaId));
        }

        var hall = await hallRepository.GetByIdAsync(command.HallId, true, ct);
        if (hall is null)
        {
            return Result.Failure<ShowtimeResponseModel>(ShowtimeErrors.HallNotFound(command.HallId));
        }

        if (hall.CinemaId != cinema.Id)
        {
            return Result.Failure<ShowtimeResponseModel>(CinemaErrors.HallNotBelongToCinema(cinema.Id, hall.Id));
        }

        var hasOverlap = await showtimeRepository.HasOverlappingAsync(
            hall.Id,
            command.StartTimeUtc,
            command.EndTimeUtc,
            excludeShowtimeId: null,
            ct);

        if (hasOverlap)
        {
            return Result.Failure<ShowtimeResponseModel>(ShowtimeErrors.OverlappingShowtime);
        }

        var showtime = Showtime.Create(
            movieId: command.MovieId,
            cinemaId: command.CinemaId,
            hallId: command.HallId,
            startTimeUtc: command.StartTimeUtc,
            endTimeUtc: command.EndTimeUtc,
            basePrice: command.BasePrice,
            currency: command.Currency,
            status: command.Status,
            language: command.Language,
            format: command.Format);

        showtimeRepository.Add(showtime, ct);
        await unitOfWork.SaveChangesAsync(ct);

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