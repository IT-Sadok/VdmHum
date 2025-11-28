namespace Application.Commands.Cinemas.UpdateCinema;

using Abstractions.Messaging;
using Application.Contracts.Cinemas;

public sealed record UpdateCinemaCommand(
    Guid Id,
    string Name,
    string City,
    string Address,
    double? Latitude,
    double? Longitude
) : ICommand<CinemaResponseModel>;