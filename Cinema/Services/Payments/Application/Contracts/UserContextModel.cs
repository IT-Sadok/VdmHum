namespace Application.Contracts;

public record UserContextModel(
    Guid? UserId,
    bool IsAuthenticated);