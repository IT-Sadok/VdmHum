namespace Application.Commands.Cinemas.CreateCinema;

using Application.Contracts.Cinemas;
using Shared.Contracts.Abstractions;

public sealed record CreateCinemaCommand(
    string Name,
    string City,
    string Address,
    double? Latitude,
    double? Longitude
) : ICommand<CinemaResponseModel>;