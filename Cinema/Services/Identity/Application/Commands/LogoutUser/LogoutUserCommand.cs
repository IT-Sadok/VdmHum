namespace Application.Commands.LogoutUser;

using Abstractions.Messaging;
using Domain.Abstractions;

public record LogoutUserCommand(
    string RefreshToken
) : ICommand<Result>;