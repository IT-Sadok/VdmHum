namespace Application.Contracts.Halls;

public sealed record HallFilter(
    Guid? CinemaId,
    string? Name);