namespace Application.Abstractions.Services;

using Contracts;

public interface IUserContextService
{
    UserContextModel GetUserContext();
}