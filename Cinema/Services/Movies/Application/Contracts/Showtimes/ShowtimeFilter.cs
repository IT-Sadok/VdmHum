namespace Application.Contracts.Showtimes;

using Domain.Enums;

public sealed record ShowtimeFilter(
    Guid? MovieId,
    Guid? CinemaId,
    Guid? HallId,
    DateTime? DateFromUtc,
    DateTime? DateToUtc,
    ShowtimeStatus? Status);