namespace Domain.Exceptions.Book;

public sealed class BookNotBorrowedException(Guid bookId)
    : DomainException($"Book with id '{bookId}' is not borrowed.")
{
    public Guid BookId { get; } = bookId;
}