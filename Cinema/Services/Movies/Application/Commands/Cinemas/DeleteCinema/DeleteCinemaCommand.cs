namespace Application.Commands.Cinemas.DeleteCinema;

using Abstractions.Messaging;

public sealed record DeleteCinemaCommand(Guid Id) : ICommand;