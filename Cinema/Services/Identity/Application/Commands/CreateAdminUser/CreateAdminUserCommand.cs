namespace Application.Commands.CreateAdminUser;

using Contracts;
using Abstractions.Messaging;

public record CreateAdminUserCommand(
    string Email,
    string Password,
    string? PhoneNumber,
    string? FirstName,
    string? LastName
) : ICommand<CreateAdminResponseModel>;