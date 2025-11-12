namespace Domain.Entities;

using Enums;
using Exceptions.Author;
using Exceptions.Book;
using ValueObjects;

public sealed class Book
{
    private readonly List<Author> _authors;

    private Book(Guid id, string title, List<Author> authors, DateOnly year, BookStatus status)
    {
        this.Id = id;
        this.Title = title;
        this._authors = authors;
        this.Year = year;
        this.Status = status;
    }

    public Guid Id { get; }

    public string Title { get; private set; }

    public IReadOnlyList<Author> Authors => this._authors;

    public DateOnly Year { get; private set; }

    public BookStatus Status { get; private set; }

    public void AddAuthor(Author author)
    {
        ArgumentNullException.ThrowIfNull(author);

        if (_authors.Any(a => a.Equals(author)))
        {
            throw new AuthorAlreadyExistsException(this.Id, author);
        }

        _authors.Add(author);
    }

    public void RemoveAuthor(Author author)
    {
        if (!_authors.Remove(author))
        {
            throw new AuthorNotFoundException(this.Id, author);
        }

        if (_authors.Count == 0)
        {
            throw new BookWithoutAuthorsException(this.Id);
        }
    }

    public void Rename(string newTitle) => this.Title = newTitle;

    public void Borrow()
    {
        if (Status == BookStatus.Borrowed)
        {
            throw new BookAlreadyBorrowedException(this.Id);
        }

        Status = BookStatus.Borrowed;
    }

    public void Return()
    {
        if (Status == BookStatus.Available)
        {
            throw new BookNotBorrowedException(this.Id);
        }

        Status = BookStatus.Available;
    }

    public static Book Create(
        string title,
        List<Author> authors,
        DateOnly year,
        BookStatus status = BookStatus.Available)
    {
        ArgumentNullException.ThrowIfNull(authors);

        if (authors.Count == 0)
        {
            throw new EmptyAuthorsListException();
        }

        return new Book(Guid.NewGuid(), title, authors, year, status);
    }
    
    public static Book Rehydrate(
        Guid id,
        string title,
        List<Author> authors,
        DateOnly year,
        BookStatus status)
    {
        return new Book(id, title, authors, year, status);
    }
}