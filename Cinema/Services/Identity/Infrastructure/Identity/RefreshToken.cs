namespace Infrastructure.Identity;

public sealed class RefreshToken
{
    public Guid Id { get; init; } = Guid.NewGuid();

    public Guid UserId { get; init; }

    public string TokenHash { get; init; } = null!;

    public DateTimeOffset ExpiresAtUtc { get; init; }

    public DateTimeOffset CreatedAtUtc { get; init; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? RevokedAtUtc { get; set; }

    public bool IsActive => this.RevokedAtUtc is null && this.ExpiresAtUtc > DateTimeOffset.UtcNow;
}