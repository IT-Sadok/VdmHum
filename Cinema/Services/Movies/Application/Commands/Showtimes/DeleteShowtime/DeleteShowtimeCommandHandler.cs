namespace Application.Commands.Showtimes.DeleteShowtime;

using Abstractions.Repositories;
using Errors;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class DeleteShowtimeCommandHandler(
    IShowtimeRepository showtimeRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteShowtimeCommand>
{
    public async Task<Result> HandleAsync(
        DeleteShowtimeCommand command,
        CancellationToken ct)
    {
        var showtime = await showtimeRepository.GetByIdAsync(command.Id, false, ct);

        if (showtime is null)
        {
            return Result.Failure(ShowtimeErrors.NotFound(command.Id));
        }

        showtimeRepository.Remove(showtime);
        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}