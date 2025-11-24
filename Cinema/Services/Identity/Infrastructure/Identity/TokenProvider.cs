namespace Infrastructure.Identity;

using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
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
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._options.SigningKey));
        var credentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email),
        };

        claims.AddRange(user.Roles.Select(role => new Claim("role", role)));

        var now = dateTimeProvider.UtcNow;

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = now.AddMinutes(this._options.AccessTokenLifetimeMinutes),
            SigningCredentials = credentials,
            Issuer = this._options.Issuer,
            Audience = this._options.Audience,
            NotBefore = now,
        };

        var handler = new JsonWebTokenHandler();

        var token = handler.CreateToken(tokenDescriptor);

        return token;
    }

    public string CreateRefreshToken()
    {
        var bytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(bytes);
    }
}