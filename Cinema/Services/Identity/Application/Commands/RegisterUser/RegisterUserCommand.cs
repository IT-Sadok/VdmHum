namespace Application.Commands.RegisterUser;

using Abstractions.Messaging;
using Contracts;

public sealed record RegisterUserCommand(
    string Email,
    string Password,
    string? PhoneNumber,
    string? FirstName,
    string? LastName
) : ICommand<AuthResponseModel>;