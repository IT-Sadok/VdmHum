namespace Domain.Exceptions.Author;

public sealed class InvalidAuthorTypeException(string type)
    : DomainException($"Invalid author type: {type}");