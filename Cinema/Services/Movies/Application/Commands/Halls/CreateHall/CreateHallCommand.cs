namespace Application.Commands.Halls.CreateHall;

using Application.Contracts.Halls;
using Shared.Contracts.Abstractions;

public sealed record CreateHallCommand(
    Guid CinemaId,
    string Name,
    int NumberOfSeats
) : ICommand<HallResponseModel>;