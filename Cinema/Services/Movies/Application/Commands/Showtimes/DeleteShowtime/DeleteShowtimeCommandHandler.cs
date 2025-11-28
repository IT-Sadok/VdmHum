namespace Application.Commands.Showtimes.DeleteShowtime;

using Abstractions.Messaging;
using Abstractions.Repositories;
using Domain.Abstractions;
using Domain.Errors;

public sealed class DeleteShowtimeCommandHandler(
    IShowtimeRepository showtimeRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteShowtimeCommand>
{
    public async Task<Result> HandleAsync(
        DeleteShowtimeCommand command,
        CancellationToken ct)
    {
        var showtime = await showtimeRepository.GetByIdAsync(command.Id, ct);

        if (showtime is null)
        {
            return Result.Failure(ShowtimeErrors.NotFound(command.Id));
        }

        showtimeRepository.Remove(showtime);
        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}