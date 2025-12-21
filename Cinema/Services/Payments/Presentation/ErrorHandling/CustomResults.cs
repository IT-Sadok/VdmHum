namespace Presentation.ErrorHandling;

using Constants;
using Shared.Contracts.Core;
using Shared.Contracts.Errors;

public static class CustomResults
{
    public static IResult Problem(Result result)
    {
        if (result.IsSuccess || result.Error is null)
        {
            throw new InvalidOperationException();
        }

        return Results.Problem(
            title: GetTitle(result.Error),
            detail: GetDetail(result.Error),
            type: GetType(result.Error.Type),
            statusCode: GetStatusCode(result.Error.Type),
            extensions: GetErrors(result));

        static string GetTitle(Error error) =>
            error.Type switch
            {
                ErrorType.Validation => error.Code,
                ErrorType.Problem => error.Code,
                ErrorType.Unauthorized => error.Code,
                ErrorType.Forbidden => error.Code,
                ErrorType.NotFound => error.Code,
                ErrorType.Conflict => error.Code,
                _ => "Server failure"
            };

        static string GetDetail(Error error) =>
            error.Type switch
            {
                ErrorType.Validation => error.Description,
                ErrorType.Problem => error.Description,
                ErrorType.Unauthorized => error.Description,
                ErrorType.Forbidden => error.Description,
                ErrorType.NotFound => error.Description,
                ErrorType.Conflict => error.Description,
                _ => "An unexpected error occurred"
            };

        static string GetType(ErrorType errorType) =>
            errorType switch
            {
                ErrorType.Validation => ErrorTypesDocumentation.Type400Validation,
                ErrorType.Problem => ErrorTypesDocumentation.Type400Problem,
                ErrorType.Unauthorized => ErrorTypesDocumentation.Type401Unauthorized,
                ErrorType.Forbidden => ErrorTypesDocumentation.Type403Forbidden,
                ErrorType.NotFound => ErrorTypesDocumentation.Type404NotFound,
                ErrorType.Conflict => ErrorTypesDocumentation.Type409Conflict,
                _ => ErrorTypesDocumentation.Type500InternalSeverError
            };

        static int GetStatusCode(ErrorType errorType) =>
            errorType switch
            {
                ErrorType.Validation or ErrorType.Problem => StatusCodes.Status400BadRequest,
                ErrorType.Unauthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Forbidden => StatusCodes.Status403Forbidden,
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

        static Dictionary<string, object?>? GetErrors(Result result)
        {
            if (result.Error is not ValidationError validationError)
            {
                return null;
            }

            return new Dictionary<string, object?>
            {
                { "errors", validationError.Errors },
            };
        }
    }
}