namespace Application.Commands.UpdateProfile;

using Abstractions.Messaging;

public record UpdateProfileCommand(
    string? PhoneNumber,
    string? FirstName,
    string? LastName
) : ICommand<Guid>;