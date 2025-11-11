namespace Domain.Exceptions.Author;

using ValueObjects;

public sealed class AuthorNotFoundException(Guid bookId, Author author)
    : DomainException($"Author '{author}' not found in book {bookId}.")
{
    public Guid BookId { get; } = bookId;
    public Author Author { get; } = author;
}