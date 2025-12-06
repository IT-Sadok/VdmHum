namespace Application.Errors;

using Shared.Contracts.Core;

public static class CommonErrors
{
    public static readonly Error Unauthorized = Error.Failure(
        "Users.Unauthorized",
        "The user are not authorized to perform this action.");
}