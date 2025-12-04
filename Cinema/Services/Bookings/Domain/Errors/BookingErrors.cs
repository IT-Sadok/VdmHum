namespace Domain.Errors;

using Abstractions;

public static class BookingErrors
{
    public static readonly Error ShowtimeNotFound = Error.NotFound(
        code: "Booking.ShowtimeNotFound",
        description: "Showtime was not found.");

    public static readonly Error SeatsUnavailable = Error.Conflict(
        code: "Booking.SeatsUnavailable",
        description: "One or more selected seats are not available.");

    public static readonly Error NotFound = Error.NotFound(
        code: "Booking.NotFound",
        description: "Booking was not found.");
}