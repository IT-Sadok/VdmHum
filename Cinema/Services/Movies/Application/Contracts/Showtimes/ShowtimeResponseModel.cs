namespace Application.Contracts.Showtimes;

using Domain.Enums;

public sealed record ShowtimeResponseModel(
    Guid Id,
    Guid MovieId,
    Guid CinemaId,
    Guid HallId,
    DateTime StartTimeUtc,
    DateTime EndTimeUtc,
    decimal BasePrice,
    string Currency,
    ShowtimeStatus Status,
    string? Language,
    string? Format,
    string? CancelReason);