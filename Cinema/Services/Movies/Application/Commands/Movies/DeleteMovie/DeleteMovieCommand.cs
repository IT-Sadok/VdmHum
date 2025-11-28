namespace Application.Commands.Movies.DeleteMovie;

using Abstractions.Messaging;

public sealed record DeleteMovieCommand(Guid Id) : ICommand;