namespace Application.Commands.Halls.DeleteHall;

using Abstractions.Messaging;

public sealed record DeleteHallCommand(Guid Id) : ICommand;