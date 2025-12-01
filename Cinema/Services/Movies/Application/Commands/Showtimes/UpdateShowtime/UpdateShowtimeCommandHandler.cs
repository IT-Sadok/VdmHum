namespace Application.Commands.Showtimes.UpdateShowtime;

using Abstractions.Messaging;
using Abstractions.Repositories;
using Contracts.Showtimes;
using Domain.Abstractions;
using Domain.Enums;
using Domain.Errors;

public sealed class UpdateShowtimeCommandHandler(
    IShowtimeRepository showtimeRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateShowtimeCommand, ShowtimeResponseModel>
{
    public async Task<Result<ShowtimeResponseModel>> HandleAsync(
        UpdateShowtimeCommand command,
        CancellationToken ct)
    {
        var showtime = await showtimeRepository.GetByIdAsync(command.Id, false, ct);

        if (showtime is null)
        {
            return Result.Failure<ShowtimeResponseModel>(ShowtimeErrors.NotFound(command.Id));
        }

        var hasOverlap = await showtimeRepository.HasOverlappingAsync(
            showtime.HallId,
            command.StartTimeUtc,
            command.EndTimeUtc,
            excludeShowtimeId: showtime.Id,
            ct);

        if (hasOverlap)
        {
            return Result.Failure<ShowtimeResponseModel>(ShowtimeErrors.OverlappingShowtime);
        }

        showtime.Reschedule(command.StartTimeUtc, command.EndTimeUtc);
        showtime.UpdateBasePrice(command.BasePrice);
        showtime.UpdateLanguage(command.Language);
        showtime.UpdateFormat(command.Format);

        if (command.Status == ShowtimeStatus.Cancelled)
        {
            showtime.Cancel(command.CancelReason);
        }
        else
        {
            showtime.ChangeStatus(command.Status);
        }

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