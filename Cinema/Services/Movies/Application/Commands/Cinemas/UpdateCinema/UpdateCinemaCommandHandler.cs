namespace Application.Commands.Cinemas.UpdateCinema;

using Abstractions.Messaging;
using Abstractions.Repositories;
using Application.Contracts.Cinemas;
using Domain.Abstractions;
using Domain.Errors;

public sealed class UpdateCinemaCommandHandler(
    ICinemaRepository cinemaRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<UpdateCinemaCommand, CinemaResponseModel>
{
    public async Task<Result<CinemaResponseModel>> HandleAsync(
        UpdateCinemaCommand command,
        CancellationToken ct)
    {
        var cinema = await cinemaRepository.GetByIdAsync(command.Id, ct);

        if (cinema is null)
        {
            return Result.Failure<CinemaResponseModel>(CinemaErrors.NotFound(command.Id));
        }

        var isNameUnique = await cinemaRepository.IsNameUniqueInCityAsync(
            command.Name,
            command.City,
            excludeCinemaId: command.Id,
            ct);

        if (!isNameUnique)
        {
            return Result.Failure<CinemaResponseModel>(CinemaErrors.NameNotUnique);
        }

        cinema.UpdateName(command.Name);
        cinema.UpdateCity(command.City);
        cinema.UpdateAddress(command.Address);

        if (command.Latitude.HasValue && command.Longitude.HasValue)
        {
            cinema.UpdateLatitudeAndLongitude(command.Latitude.Value, command.Longitude.Value);
        }

        await unitOfWork.SaveChangesAsync(ct);

        var response = new CinemaResponseModel(
            cinema.Id,
            cinema.Name,
            cinema.City,
            cinema.Address,
            cinema.Latitude,
            cinema.Longitude);

        return response;
    }
}