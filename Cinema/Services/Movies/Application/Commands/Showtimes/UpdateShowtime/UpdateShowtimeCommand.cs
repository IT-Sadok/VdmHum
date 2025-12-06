namespace Application.Commands.Showtimes.UpdateShowtime;

using Contracts.Showtimes;
using Domain.Enums;
using Shared.Contracts.Abstractions;

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