namespace Application.Commands.UpdateProfile;

using Shared.Contracts.Abstractions;

public record UpdateProfileCommand(
    string? PhoneNumber,
    string? FirstName,
    string? LastName
) : ICommand<Guid>;