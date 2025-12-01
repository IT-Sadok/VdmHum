namespace Application.Commands.Halls.CreateHall;

using Abstractions.Messaging;
using Application.Contracts.Halls;

public sealed record CreateHallCommand(
    Guid CinemaId,
    string Name,
    int NumberOfSeats
) : ICommand<HallResponseModel>;