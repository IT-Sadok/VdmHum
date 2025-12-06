namespace Application.Commands.Showtimes.DeleteShowtime;

using Shared.Contracts.Abstractions;

public sealed record DeleteShowtimeCommand(Guid Id) : ICommand;