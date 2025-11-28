namespace Application.Commands.Cinemas.DeleteCinema;

using Abstractions.Messaging;
using Abstractions.Repositories;
using Domain.Abstractions;
using Domain.Errors;

public sealed class DeleteCinemaCommandHandler(
    ICinemaRepository cinemaRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteCinemaCommand>
{
    public async Task<Result> HandleAsync(
        DeleteCinemaCommand command,
        CancellationToken ct)
    {
        var cinema = await cinemaRepository.GetByIdAsync(command.Id, ct);

        if (cinema is null)
        {
            return Result.Failure(CinemaErrors.NotFound(command.Id));
        }

        cinemaRepository.Remove(cinema);
        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}