namespace Application.Abstractions.Services;

public interface IUserContextService
{
    Guid? UserId { get; }

    bool IsAuthenticated { get; }
}