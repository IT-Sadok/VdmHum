namespace Application.Commands.Cinemas.CreateCinema;

using Abstractions.Messaging;
using Application.Contracts.Cinemas;

public sealed record CreateCinemaCommand(
    string Name,
    string City,
    string Address,
    double? Latitude,
    double? Longitude
) : ICommand<CinemaResponseModel>;