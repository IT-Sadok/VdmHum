namespace Application.Commands.Halls.DeleteHall;

using Shared.Contracts.Abstractions;

public sealed record DeleteHallCommand(Guid Id) : ICommand;