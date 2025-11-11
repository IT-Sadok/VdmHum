namespace Domain.Exceptions.Author;

public sealed class InvalidAuthorNameException(string reason)
    : DomainException($"Invalid author name: {reason}");