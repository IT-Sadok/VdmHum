namespace Application.Commands.Movies.DeleteMovie;

using Shared.Contracts.Abstractions;

public sealed record DeleteMovieCommand(Guid Id) : ICommand;