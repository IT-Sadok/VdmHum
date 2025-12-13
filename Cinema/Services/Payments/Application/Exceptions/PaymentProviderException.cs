namespace Application.Exceptions;

using System.Net;

public sealed class PaymentProviderException(
    string message,
    HttpStatusCode statusCode,
    Exception? innerException = null
) : Exception(message, innerException)
{
    public HttpStatusCode StatusCode { get; } = statusCode;
}