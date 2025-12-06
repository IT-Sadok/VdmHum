namespace Application.Commands.RegisterUser;

using Contracts;
using Shared.Contracts.Abstractions;

public sealed record RegisterUserCommand(
    string Email,
    string Password,
    string? PhoneNumber,
    string? FirstName,
    string? LastName
) : ICommand<AuthResponseModel>;