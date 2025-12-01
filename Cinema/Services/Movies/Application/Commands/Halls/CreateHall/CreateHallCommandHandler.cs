namespace Application.Commands.Halls.CreateHall;

using Abstractions.Messaging;
using Abstractions.Repositories;
using Application.Contracts.Halls;
using Domain.Abstractions;
using Domain.Entities;
using Domain.Errors;

public sealed class CreateHallCommandHandler(
    IHallRepository hallRepository,
    ICinemaRepository cinemaRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateHallCommand, HallResponseModel>
{
    public async Task<Result<HallResponseModel>> HandleAsync(
        CreateHallCommand command,
        CancellationToken ct)
    {
        var cinema = await cinemaRepository.GetByIdAsync(command.CinemaId, true, ct);

        if (cinema is null)
        {
            return Result.Failure<HallResponseModel>(HallErrors.CinemaNotFound(command.CinemaId));
        }

        var isNameUnique = await hallRepository.IsNameUniquePerCinemaAsync(
            command.CinemaId,
            command.Name,
            excludeHallId: null,
            ct);

        if (!isNameUnique)
        {
            return Result.Failure<HallResponseModel>(HallErrors.NameNotUniqueWithinCinema);
        }

        var hall = Hall.Create(
            cinemaId: command.CinemaId,
            name: command.Name,
            numberOfSeats: command.NumberOfSeats);

        hallRepository.Add(hall, ct);

        await unitOfWork.SaveChangesAsync(ct);

        var response = new HallResponseModel(
            hall.Id,
            hall.CinemaId,
            hall.Name,
            hall.NumberOfSeats);

        return response;
    }
}