namespace Application.Contracts;

public record UserResponseModel(
    Guid UserId,
    string Email,
    string? PhoneNumber,
    string? FirstName,
    string? LastName,
    IReadOnlyCollection<string> Roles);