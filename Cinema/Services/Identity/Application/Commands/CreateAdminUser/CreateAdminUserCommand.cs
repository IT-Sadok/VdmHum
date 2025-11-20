namespace Application.Commands.CreateAdminUser;

using Abstractions.Messaging;

public record CreateAdminUserCommand(
    string Email,
    string Password,
    string? PhoneNumber,
    string? FirstName,
    string? LastName
) : ICommand<Guid>;