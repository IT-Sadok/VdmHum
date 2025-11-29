namespace Application.Commands.Halls.UpdateHall;

using Abstractions.Messaging;
using Abstractions.Repositories;
using Application.Contracts.Halls;
using Domain.Abstractions;
using Domain.Errors;

public sealed class UpdateHallCommandHandler(
    IHallRepository hallRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateHallCommand, HallResponseModel>
{
    public async Task<Result<HallResponseModel>> HandleAsync(
        UpdateHallCommand command,
        CancellationToken ct)
    {
        var hall = await hallRepository.GetByIdAsync(command.Id, false, ct);

        if (hall is null)
        {
            return Result.Failure<HallResponseModel>(HallErrors.NotFound(command.Id));
        }

        var isNameUnique = await hallRepository.IsNameUniqueWithinCinemaAsync(
            hall.CinemaId,
            command.Name,
            hall.Id,
            ct);

        if (!isNameUnique)
        {
            return Result.Failure<HallResponseModel>(HallErrors.NameNotUniqueWithinCinema);
        }

        hall.UpdateName(command.Name);
        hall.UpdateNumberOfSeats(command.NumberOfSeats);

        await unitOfWork.SaveChangesAsync(ct);

        var response = new HallResponseModel(
            hall.Id,
            hall.CinemaId,
            hall.Name,
            hall.NumberOfSeats);

        return response;
    }
}