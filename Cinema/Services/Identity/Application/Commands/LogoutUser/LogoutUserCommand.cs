namespace Application.Commands.LogoutUser;

using Shared.Contracts.Abstractions;
using Shared.Contracts.Core;

public record LogoutUserCommand(
    string RefreshToken
) : ICommand;