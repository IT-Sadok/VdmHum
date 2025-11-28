namespace Application.Commands.Showtimes.CreateShowtime;

using Abstractions.Messaging;
using Contracts.Showtimes;
using Domain.Enums;

public sealed record CreateShowtimeCommand(
    Guid MovieId,
    Guid CinemaId,
    Guid HallId,
    DateTime StartTimeUtc,
    DateTime EndTimeUtc,
    decimal BasePrice,
    string Currency,
    ShowtimeStatus Status = ShowtimeStatus.Scheduled,
    string? Language = null,
    string? Format = null
) : ICommand<ShowtimeResponseModel>;