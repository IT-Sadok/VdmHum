namespace Application.Errors;

using Shared.Contracts.Core;

public static class HallErrors
{
    public static readonly Error Unauthorized = Error.Failure(
        "Halls.Unauthorized",
        "The user is not authorized to perform this action on halls.");

    public static readonly Error InvalidName = Error.Problem(
        "Halls.InvalidName",
        "Name is required.");

    public static readonly Error InvalidNumberOfSeats = Error.Problem(
        "Halls.InvalidNumberOfSeats",
        "Number of seats must be a positive integer.");

    public static readonly Error NameNotUniqueWithinCinema = Error.Conflict(
        "Halls.NameNotUniqueWithinCinema",
        "A hall with the same name already exists in this cinema.");

    public static Error NotFound(Guid hallId) => Error.NotFound(
        "Halls.NotFound",
        $"The hall with the Id = '{hallId}' was not found.");

    public static Error CinemaNotFound(Guid cinemaId) => Error.NotFound(
        "Halls.CinemaNotFound",
        $"The cinema with the Id = '{cinemaId}' was not found.");
}