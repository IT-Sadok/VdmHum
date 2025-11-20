namespace Application.Contracts;

public record AuthResponseModel(Guid UserId, string AccessToken, string RefreshToken);