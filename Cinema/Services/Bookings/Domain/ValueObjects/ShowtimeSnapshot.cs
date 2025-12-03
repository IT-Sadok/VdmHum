namespace Domain.ValueObjects;

public sealed record ShowtimeSnapshot(
    Guid ShowtimeId,
    string MovieTitle,
    string CinemaName,
    string HallName,
    DateTime StartTimeUtc);