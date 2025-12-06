namespace Presentation.Infrastructure.Constants;

public static class ErrorTypesDocumentation
{
    public const string Type400Validation = $"{Base}6.5.1";

    public const string Type400Problem = $"{Base}6.5.1";

    public const string Type404NotFound = $"{Base}6.5.4";

    public const string Type409Conflict = $"{Base}6.5.8";

    public const string Type500InternalSeverError = $"{Base}6.6.1";

    private const string Base = "https://datatracker.ietf.org/doc/html/rfc7231#section-";
}