namespace Application.Abstractions.Providers;

using Domain.Entities;

public interface ITokenProvider
{
    string CreateAccessToken(User user);

    string CreateRefreshToken();
}