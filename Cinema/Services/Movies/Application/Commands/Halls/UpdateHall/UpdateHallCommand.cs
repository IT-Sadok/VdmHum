namespace Application.Commands.Halls.UpdateHall;

using Abstractions.Messaging;
using Application.Contracts.Halls;

public sealed record UpdateHallCommand(
    Guid Id,
    string Name,
    int NumberOfSeats
) : ICommand<HallResponseModel>;