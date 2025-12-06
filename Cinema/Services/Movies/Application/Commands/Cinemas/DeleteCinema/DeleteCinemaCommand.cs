namespace Application.Commands.Cinemas.DeleteCinema;

using Shared.Contracts.Abstractions;

public sealed record DeleteCinemaCommand(Guid Id) : ICommand;