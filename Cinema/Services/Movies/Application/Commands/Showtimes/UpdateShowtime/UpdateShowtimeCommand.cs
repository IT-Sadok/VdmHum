namespace Application.Commands.Showtimes.UpdateShowtime;

using Abstractions.Messaging;
using Contracts.Showtimes;
using Domain.Enums;

public sealed record UpdateShowtimeCommand(
    Guid Id,
    DateTime StartTimeUtc,
    DateTime EndTimeUtc,
    decimal BasePrice,
    ShowtimeStatus Status,
    string? Language,
    string? Format,
    string? CancelReason
) : ICommand<ShowtimeResponseModel>;