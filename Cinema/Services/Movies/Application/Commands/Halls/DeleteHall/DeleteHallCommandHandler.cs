namespace Application.Commands.Halls.DeleteHall;

using Abstractions.Messaging;
using Abstractions.Repositories;
using Domain.Abstractions;
using Domain.Errors;

public sealed class DeleteHallCommandHandler(
    IHallRepository hallRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<DeleteHallCommand>
{
    public async Task<Result> HandleAsync(
        DeleteHallCommand command,
        CancellationToken ct)
    {
        var hall = await hallRepository.GetByIdAsync(command.Id, false, ct);

        if (hall is null)
        {
            return Result.Failure(HallErrors.NotFound(command.Id));
        }

        hallRepository.Remove(hall);
        await unitOfWork.SaveChangesAsync(ct);

        return Result.Success();
    }
}