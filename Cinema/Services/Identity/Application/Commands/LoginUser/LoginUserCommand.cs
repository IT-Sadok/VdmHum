namespace Application.Commands.LoginUser;

using Abstractions.Messaging;
using Contracts;

public sealed record LoginUserCommand(
    string Email,
    string Password
) : ICommand<AuthResponseModel>;