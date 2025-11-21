namespace Application.Options;

public sealed class JwtOptions
{
    public string Issuer { get; init; } = null!;

    public string Audience { get; init; } = null!;

    public string SigningKey { get; init; } = null!;

    public string AccessTokenName { get; init; } = "access_token";

    public string RefreshTokenName { get; init; } = "refresh_token";

    public int AccessTokenLifetimeMinutes { get; init; } = 15;

    public int RefreshTokenLifetimeDays { get; init; } = 30;
}