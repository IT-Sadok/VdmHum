namespace Application.Errors;

using Shared.Contracts.Core;

public static class CinemaErrors
{
    public static readonly Error Unauthorized = Error.Failure(
        "Cinemas.Unauthorized",
        "The user is not authorized to perform this action on cinemas.");

    public static readonly Error NameNotUnique = Error.Conflict(
        "Cinemas.NameNotUnique",
        "The provided cinema name is not unique in this city.");

    public static readonly Error InvalidCoordinates = Error.Problem(
        "Cinemas.InvalidCoordinates",
        "You must specify both latitude and longitude, or specify neither.");

    public static Error NotFound(Guid cinemaId) => Error.NotFound(
        "Cinemas.NotFound",
        $"The cinema with the Id = '{cinemaId}' was not found.");

    public static Error HallNotBelongToCinema(Guid cinemaId, Guid hallId) => Error.Conflict(
        "Cinemas.HallNotBelongToCinema",
        $"The hall with Id = '{hallId}' does not belong to the cinema with Id = '{cinemaId}'.");
}