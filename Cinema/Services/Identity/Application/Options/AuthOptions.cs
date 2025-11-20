namespace Application.Options;

public sealed class AuthOptions
{
    public int RefreshTokenLifetimeDays { get; init; } = 30;
}