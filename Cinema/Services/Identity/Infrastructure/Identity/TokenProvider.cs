namespace Infrastructure.Identity;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Application.Options;
using Application.Abstractions.Providers;
using Domain.Entities;
using Microsoft.Extensions.Options;

public sealed class TokenProvider(
    IOptions<JwtOptions> options,
    IDateTimeProvider dateTimeProvider)
    : ITokenProvider
{
    private readonly JwtOptions _options = options.Value;

    public string CreateAccessToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new(ClaimTypes.Name, user.Email),
        };

        claims.AddRange(user.Roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._options.SigningKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var now = dateTimeProvider.UtcNow;

        var token = new JwtSecurityToken(
            issuer: this._options.Issuer,
            audience: this._options.Audience,
            claims: claims,
            notBefore: now,
            expires: now.AddMinutes(this._options.AccessTokenLifetimeMinutes),
            signingCredentials: creds);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public string CreateRefreshToken(User user)
    {
        var bytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(bytes);
    }
}