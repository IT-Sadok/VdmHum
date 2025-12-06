namespace Application.Commands.RefreshToken;

using Contracts;
using Shared.Contracts.Abstractions;

public record RefreshTokenCommand(
    string RefreshToken
) : ICommand<AuthResponseModel>;