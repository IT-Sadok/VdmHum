namespace Domain.Exceptions.Author;

public sealed class EmptyAuthorsListException()
    : DomainException("Authors list cannot be empty.");