namespace Application.Errors;

using Shared.Contracts.Core;

public static class ShowtimeErrors
{
    public static readonly Error Unauthorized = Error.Failure(
        "Showtimes.Unauthorized",
        "The user is not authorized to perform this action on showtimes.");

    public static readonly Error InvalidTimeRange = Error.Problem(
        "Showtimes.InvalidTimeRange",
        "End time must be greater than start time.");

    public static readonly Error InvalidBasePrice = Error.Problem(
        "Showtimes.InvalidBasePrice",
        "Base price cannot be negative.");

    public static readonly Error InvalidCurrency = Error.Problem(
        "Showtimes.InvalidCurrency",
        "Currency must be a 3-letter ISO 4217 code.");

    public static readonly Error InvalidStatusTransition = Error.Conflict(
        "Showtimes.InvalidStatusTransition",
        "The showtime status transition is not allowed.");

    public static readonly Error CannotRescheduleCancelled = Error.Conflict(
        "Showtimes.CannotRescheduleCancelled",
        "Cannot reschedule a cancelled showtime.");

    public static readonly Error CannotChangePriceForCancelled = Error.Conflict(
        "Showtimes.CannotChangePriceForCancelled",
        "Cannot change the price of a cancelled showtime.");

    public static readonly Error CannotChangeStatusOfCancelled = Error.Conflict(
        "Showtimes.CannotChangeStatusOfCancelled",
        "Cannot change the status of a cancelled showtime.");

    public static readonly Error OnlyActiveCanBeSoldOut = Error.Conflict(
        "Showtimes.OnlyActiveCanBeSoldOut",
        "Only an active showtime can be marked as sold out.");

    public static readonly Error OverlappingShowtime = Error.Conflict(
        "Showtimes.OverlappingShowtime",
        "The showtime overlaps with an existing showtime in the same hall.");

    public static Error NotFound(Guid showtimeId) => Error.NotFound(
        "Showtimes.NotFound",
        $"The showtime with the Id = '{showtimeId}' was not found.");

    public static Error MovieNotFound(Guid movieId) => Error.NotFound(
        "Showtimes.MovieNotFound",
        $"The movie with the Id = '{movieId}' was not found.");

    public static Error HallNotFound(Guid hallId) => Error.NotFound(
        "Showtimes.HallNotFound",
        $"The hall with the Id = '{hallId}' was not found.");

    public static Error CinemaNotFound(Guid cinemaId) => Error.NotFound(
        "Showtimes.CinemaNotFound",
        $"The cinema with the Id = '{cinemaId}' was not found.");
}