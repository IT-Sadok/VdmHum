namespace Application.Errors;

using Shared.Contracts.Core;

public static class CommonErrors
{
    public static readonly Error NotFound = Error.NotFound(
        code: "CommonError.NotFound",
        description: "Entity not found.");

    public static readonly Error Forbidden = Error.Forbidden(
        "Users.Forbidden",
        "User do not have permission to do that.");
}