namespace Application.Commands.Cinemas.CreateCinema;

using Abstractions.Repositories;
using Application.Contracts.Cinemas;
using Domain.Entities;
using Errors;
using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public sealed class CreateCinemaCommandHandler(
    ICinemaRepository cinemaRepository,
    IUnitOfWork unitOfWork)
    : ICommandHandler<CreateCinemaCommand, CinemaResponseModel>
{
    public async Task<Result<CinemaResponseModel>> HandleAsync(
        CreateCinemaCommand command,
        CancellationToken ct)
    {
        var isNameUnique = await cinemaRepository.IsNameUniquePerCityAsync(
            command.Name,
            command.City,
            excludeCinemaId: null,
            ct);

        if (!isNameUnique)
        {
            return Result.Failure<CinemaResponseModel>(CinemaErrors.NameNotUnique);
        }

        var cinema = Cinema.Create(
            name: command.Name,
            city: command.City,
            address: command.Address,
            latitude: command.Latitude,
            longitude: command.Longitude);

        cinemaRepository.Add(cinema, ct);
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