namespace Application.Errors;

using Shared.Contracts.Core;

public static class CommonErrors
{
    public static readonly Error InvalidId = Error.Failure(
        code: "CommonError.InvalidBookingId",
        description: "Id is invalid.");

    public static readonly Error InvalidAmount = Error.Failure(
        code: "CommonError.InvalidAmount",
        description: "Amount must be greater than zero.");

    public static readonly Error NotFound = Error.NotFound(
        code: "CommonError.NotFound",
        description: "Entity not found.");
}