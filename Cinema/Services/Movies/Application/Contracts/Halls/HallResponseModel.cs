namespace Application.Contracts.Halls;

public sealed record HallResponseModel(
    Guid Id,
    Guid CinemaId,
    string Name,
    int NumberOfSeats);