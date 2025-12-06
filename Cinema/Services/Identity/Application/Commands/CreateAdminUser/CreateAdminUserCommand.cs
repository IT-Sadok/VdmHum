namespace Application.Commands.CreateAdminUser;

using Contracts;
using Shared.Contracts.Abstractions;

public record CreateAdminUserCommand(
    string Email,
    string Password,
    string? PhoneNumber,
    string? FirstName,
    string? LastName
) : ICommand<CreateAdminResponseModel>;