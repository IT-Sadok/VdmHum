namespace Application.Commands.RefreshToken;

using Abstractions.Messaging;
using Contracts;

public record RefreshTokenCommand(
    string RefreshToken
) : ICommand<AuthResponseModel>;