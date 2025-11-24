namespace Application.Abstractions.Providers;

public interface IUserContext
{
    Guid? UserId { get; }

    bool IsAuthenticated { get; }
}