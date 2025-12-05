namespace Domain.ValueObjects;

public sealed record ShowtimeSnapshot(
    Guid ShowtimeId,
    Guid MovieId,
    Guid CinemaId,
    Guid HallId,
    string MovieTitle,
    string CinemaName,
    string HallName,
    DateTime StartTimeUtc);