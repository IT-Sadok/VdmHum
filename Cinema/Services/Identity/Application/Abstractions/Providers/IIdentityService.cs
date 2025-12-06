namespace Application.Abstractions.Providers;

using Errors.Entities;

public interface IIdentityService
{
    Task<User?> GetByIdAsync(Guid userId, CancellationToken ct = default);

    Task<User?> FindByEmailAsync(string email, CancellationToken ct = default);

    Task<bool> CheckPasswordAsync(User user, string password, CancellationToken ct = default);

    Task<IReadOnlyCollection<string>> GetRolesAsync(User user, CancellationToken ct = default);

    Task<User> RegisterAsync(
        string email,
        string password,
        string? phoneNumber,
        string? firstName,
        string? lastName,
        CancellationToken ct = default);

    Task UpdateProfileAsync(
        Guid userId,
        string? phoneNumber,
        string? firstName,
        string? lastName,
        CancellationToken ct = default);

    Task AddToRoleAsync(User user, string role, CancellationToken ct = default);

    Task StoreRefreshTokenAsync(
        Guid userId,
        string refreshToken,
        DateTime expiresAtUtc,
        CancellationToken ct = default);

    Task<User?> GetUserByRefreshTokenAsync(string refreshToken, CancellationToken ct = default);

    Task<bool> TryRevokeRefreshTokenAsync(string refreshToken, CancellationToken ct = default);
}