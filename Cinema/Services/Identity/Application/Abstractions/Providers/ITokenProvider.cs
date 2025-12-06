namespace Application.Abstractions.Providers;

using Errors.Entities;

public interface ITokenProvider
{
    string CreateAccessToken(User user);

    string CreateRefreshToken();
}