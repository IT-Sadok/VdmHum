namespace Domain.Exceptions.BookTitle;

public sealed class InvalidBookTitleException(string reason)
    : DomainException($"Invalid book title: {reason}");