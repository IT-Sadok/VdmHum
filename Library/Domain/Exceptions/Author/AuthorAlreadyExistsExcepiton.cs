namespace Domain.Exceptions.Author;

using ValueObjects;

public sealed class AuthorAlreadyExistsException(Guid bookId, Author author)
    : DomainException($"Author '{author}' already exists in book {bookId}.")
{
    public Guid BookId { get; } = bookId;
    public Author Author { get; } = author;
}