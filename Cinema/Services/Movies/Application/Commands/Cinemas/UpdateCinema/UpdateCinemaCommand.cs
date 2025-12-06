namespace Application.Commands.Cinemas.UpdateCinema;

using Application.Contracts.Cinemas;
using Shared.Contracts.Abstractions;

public sealed record UpdateCinemaCommand(
    Guid Id,
    string Name,
    string City,
    string Address,
    double? Latitude,
    double? Longitude
) : ICommand<CinemaResponseModel>;