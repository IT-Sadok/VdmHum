namespace Domain.Exceptions.Author;

public sealed class BookWithoutAuthorsException(Guid bookId)
    : DomainException($"Book {bookId} must have at least one author (or Anonymous).")
{
    public Guid BookId { get; } = bookId;
}