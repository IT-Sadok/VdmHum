namespace Application.Options;

public sealed class JwtOptions
{
    public string Issuer { get; init; } = null!;

    public string Audience { get; init; } = null!;

    public string SigningCertificatePath { get; init; } = null!;
}