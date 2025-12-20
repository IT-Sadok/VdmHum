namespace Presentation.Extensions;

using Grpc.Core;
using Shared.Contracts.Core;

public static class GrpcResultExtensions
{
    public static RpcException ToRpcException(this Result result)
    {
        if (result.IsSuccess || result.Error is null)
        {
            throw new InvalidOperationException("Cannot create RpcException from successful result.");
        }

        var statusCode = MapErrorTypeToStatusCode(result.Error.Type);

        return new RpcException(new Status(statusCode, result.Error.Description));
    }

    private static StatusCode MapErrorTypeToStatusCode(ErrorType errorType) =>
        errorType switch
        {
            ErrorType.Validation or ErrorType.Problem => StatusCode.InvalidArgument,
            ErrorType.Unauthorized => StatusCode.Unauthenticated,
            ErrorType.Forbidden => StatusCode.PermissionDenied,
            ErrorType.NotFound => StatusCode.NotFound,
            ErrorType.Conflict => StatusCode.Aborted,
            _ => StatusCode.Internal
        };
}