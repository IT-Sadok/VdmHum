namespace Application.Errors;

using Shared.Contracts.Core;

public static class BookingErrors
{
    public static readonly Error ShowtimeNotFound = Error.NotFound(
        code: "Booking.ShowtimeNotFound",
        description: ValidationMessages.NotFound);

    public static readonly Error SeatsUnavailable = Error.Conflict(
        code: "Booking.SeatsUnavailable",
        description: "One or more selected seats are not available.");

    public static readonly Error NotFound = Error.NotFound(
        code: "Booking.NotFound",
        description: ValidationMessages.NotFound);

    public static readonly Error UserIdNotMatch = Error.Failure(
        code: "Booking.UserIdNotMatch",
        description: "UserId does not match in booking.");
}