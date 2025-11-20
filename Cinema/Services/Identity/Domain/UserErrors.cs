namespace Domain;

using Abstractions;

public static class UserErrors
{
    public static readonly Error Unauthorized = Error.Failure(
        "Users.Unauthorized",
        "The user are not authorized to perform this action.");

    public static readonly Error InvalidCredentials = Error.NotFound(
        "Users.InvalidCredentials",
        "The user with such credentials are not found.");

    public static readonly Error EmailNotUnique = Error.Conflict(
        "Users.EmailNotUnique",
        "The provided email is not unique");

    public static readonly Error InvalidRefreshToken = Error.NotFound(
        "Users.InvalidRefreshToken",
        "The refresh token is not found.");

    public static Error NotFound(Guid userId) => Error.NotFound(
        "Users.NotFound",
        $"The user with the Id = '{userId}' was not found");
}