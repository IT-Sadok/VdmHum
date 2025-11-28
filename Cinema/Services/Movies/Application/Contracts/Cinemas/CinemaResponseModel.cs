namespace Application.Contracts.Cinemas;

public sealed record CinemaResponseModel(
    Guid Id,
    string Name,
    string City,
    string Address,
    double? Latitude,
    double? Longitude);