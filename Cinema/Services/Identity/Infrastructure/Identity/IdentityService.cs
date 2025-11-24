namespace Infrastructure.Identity;

using System.Security.Cryptography;
using System.Text;
using Application.Abstractions.Providers;
using Domain.Entities;
using Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

public sealed class IdentityService(
    UserManager<ApplicationUser> userManager,
    RoleManager<IdentityRole<Guid>> roleManager,
    ApplicationDbContext dbContext)
    : IIdentityService
{
    public async Task<User?> GetByIdAsync(Guid userId, CancellationToken ct = default)
    {
        var appUser = await userManager.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == userId, ct);

        if (appUser is null)
        {
            return null;
        }

        var roles = await userManager.GetRolesAsync(appUser);
        return MapToDomainUser(appUser, roles);
    }

    public async Task<User?> FindByEmailAsync(string email, CancellationToken ct = default)
    {
        var appUser = await userManager.FindByEmailAsync(email);
        if (appUser is null)
        {
            return null;
        }

        var roles = await userManager.GetRolesAsync(appUser);
        return MapToDomainUser(appUser, roles);
    }

    public async Task<bool> CheckPasswordAsync(User user, string password, CancellationToken ct = default)
    {
        var appUser = await userManager.FindByIdAsync(user.Id.ToString());
        if (appUser is null)
        {
            return false;
        }

        return await userManager.CheckPasswordAsync(appUser, password);
    }

    public async Task<IReadOnlyCollection<string>> GetRolesAsync(User user, CancellationToken ct = default)
    {
        var appUser = await userManager.FindByIdAsync(user.Id.ToString());
        if (appUser is null)
        {
            return [];
        }

        var roles = await userManager.GetRolesAsync(appUser);
        return (IReadOnlyCollection<string>)roles;
    }

    public async Task<User> RegisterAsync(
        string email,
        string password,
        string? phoneNumber,
        string? firstName,
        string? lastName,
        CancellationToken ct = default)
    {
        var appUser = new ApplicationUser
        {
            Id = Guid.CreateVersion7(),
            Email = email,
            UserName = email,
            PhoneNumber = phoneNumber,
            FirstName = firstName,
            LastName = lastName,
        };

        var identityResult = await userManager.CreateAsync(appUser, password);
        if (identityResult.Succeeded)
        {
            return MapToDomainUser(appUser);
        }

        var errors = string.Join(", ", identityResult.Errors.Select(e => e.Description));
        throw new InvalidOperationException($"Failed to create user: {errors}");
    }

    public async Task UpdateProfileAsync(
        Guid userId,
        string? phoneNumber,
        string? firstName,
        string? lastName,
        CancellationToken ct = default)
    {
        var appUser = await userManager.FindByIdAsync(userId.ToString())
                      ?? throw new InvalidOperationException("User not found");

        appUser.PhoneNumber = phoneNumber;
        appUser.FirstName = firstName;
        appUser.LastName = lastName;

        var result = await userManager.UpdateAsync(appUser);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to update user: {errors}");
        }
    }

    public async Task AddToRoleAsync(User user, string role, CancellationToken ct = default)
    {
        var appUser = await userManager.FindByIdAsync(user.Id.ToString())
                      ?? throw new InvalidOperationException("User not found");

        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }

        var result = await userManager.AddToRoleAsync(appUser, role);
        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to add role: {errors}");
        }
    }

    public async Task StoreRefreshTokenAsync(
        Guid userId,
        string refreshToken,
        DateTime expiresAtUtc,
        CancellationToken ct = default)
    {
        var tokenHash = ComputeHash(refreshToken);

        var entity = new RefreshToken
        {
            UserId = userId,
            TokenHash = tokenHash,
            ExpiresAtUtc = new DateTimeOffset(expiresAtUtc, TimeSpan.Zero),
        };

        dbContext.Add(entity);
        await dbContext.SaveChangesAsync(ct);
    }

    public async Task<User?> GetUserByRefreshTokenAsync(
        string refreshToken,
        CancellationToken ct = default)
    {
        var tokenHash = ComputeHash(refreshToken);

        var token = await dbContext.Set<RefreshToken>()
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.TokenHash == tokenHash &&
                     x.RevokedAtUtc == null &&
                     x.ExpiresAtUtc > DateTimeOffset.UtcNow,
                ct);

        if (token is null)
        {
            return null;
        }

        var appUser = await userManager.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == token.UserId, ct);

        if (appUser is null)
        {
            return null;
        }

        var roles = await userManager.GetRolesAsync(appUser);
        return MapToDomainUser(appUser, roles);
    }

    public async Task<bool> TryRevokeRefreshTokenAsync(
        string refreshToken,
        CancellationToken ct = default)
    {
        var hash = ComputeHash(refreshToken);

        var affected = await dbContext.Set<RefreshToken>()
            .Where(t => t.TokenHash == hash
                        && t.RevokedAtUtc == null
                        && t.ExpiresAtUtc > DateTimeOffset.UtcNow)
            .ExecuteUpdateAsync(
                setters => setters
                    .SetProperty(t => t.RevokedAtUtc, DateTimeOffset.UtcNow), ct);

        return affected == 1;
    }

    private static string ComputeHash(string value)
    {
        var bytes = Encoding.UTF8.GetBytes(value);
        var hash = SHA256.HashData(bytes);
        return Convert.ToBase64String(hash);
    }

    private static User MapToDomainUser(ApplicationUser user, IEnumerable<string>? roles = null)
        => User.FromIdentity(
            user.Id,
            user.Email!,
            user.PhoneNumber,
            user.FirstName,
            user.LastName,
            roles);
}