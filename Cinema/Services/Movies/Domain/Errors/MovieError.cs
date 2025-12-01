namespace Domain.Errors;

using Abstractions;

public static class MovieErrors
{
    public static readonly Error Unauthorized = Error.Failure(
        "Movies.Unauthorized",
        "The user is not authorized to perform this action on movies.");

    public static readonly Error TitleNotUnique = Error.Conflict(
        "Movies.TitleNotUnique",
        "The provided movie title is not unique.");

    public static readonly Error InvalidTitle = Error.Problem(
        "Movies.InvalidTitle",
        "Title is required.");

    public static readonly Error InvalidDuration = Error.Problem(
        "Movies.InvalidDuration",
        "Duration must be a positive number of minutes.");

    public static readonly Error InvalidPosterUrl = Error.Problem(
        "Movies.InvalidPosterUrl",
        "Poster URL must be a valid absolute URL.");

    public static readonly Error InvalidReleaseDate = Error.Problem(
        "Movies.InvalidReleaseDate",
        "Release date is required.");

    public static readonly Error InvalidStatusTransition = Error.Conflict(
        "Movies.InvalidStatusTransition",
        "The movie status transition is not allowed.");

    public static Error NotFound(Guid movieId) => Error.NotFound(
        "Movies.NotFound",
        $"The movie with the Id = '{movieId}' was not found.");
}