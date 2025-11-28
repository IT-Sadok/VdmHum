namespace Application.Commands.Showtimes.DeleteShowtime;

using Abstractions.Messaging;

public sealed record DeleteShowtimeCommand(Guid Id) : ICommand;