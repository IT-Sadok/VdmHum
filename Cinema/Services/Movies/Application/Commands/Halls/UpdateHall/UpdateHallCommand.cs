namespace Application.Commands.Halls.UpdateHall;

using Application.Contracts.Halls;
using Shared.Contracts.Abstractions;

public sealed record UpdateHallCommand(
    Guid Id,
    string Name,
    int NumberOfSeats
) : ICommand<HallResponseModel>;