namespace Application.Commands.LoginUser;

using Contracts;
using Shared.Contracts.Abstractions;

public sealed record LoginUserCommand(
    string Email,
    string Password
) : ICommand<AuthResponseModel>;