namespace Domain.Exceptions.Book;

public sealed class BookAlreadyBorrowedException(Guid bookId)
    : DomainException($"Book with id '{bookId}' is already borrowed.")
{
    public Guid BookId { get; } = bookId;
}