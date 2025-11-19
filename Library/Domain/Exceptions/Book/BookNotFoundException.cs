namespace Domain.Exceptions.Book;

public sealed class BookNotFoundException(Guid bookId)
    : DomainException($"Book with id '{bookId}' not found.")
{
    public Guid BookId { get; } = bookId;
}