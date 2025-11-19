namespace Domain.Entities;

using Enums;
using ValueObjects;

public sealed class Book
{
    private readonly HashSet<Author> _authors;

    private Book(Guid id, string title, HashSet<Author> authors, DateOnly year, BookStatus status)
    {
        this.Id = id;
        this.Title = title;
        this._authors = authors;
        this.Year = year;
        this.Status = status;
    }

    public Guid Id { get; }

    public string Title { get; private set; }

    public IEnumerable<Author> Authors => this._authors;

    public DateOnly Year { get; private set; }

    public BookStatus Status { get; private set; }

    public void AddAuthor(Author author)
    {
        ArgumentNullException.ThrowIfNull(author);

        if (!_authors.Add(author))
        {
            throw new InvalidOperationException(
                $"Author {author.Name} {author.Type} already added to book {Id}");
        }
    }

    public void RemoveAuthor(Author author)
    {
        ArgumentNullException.ThrowIfNull(author);

        if (_authors.Count == 1 && _authors.Contains(author))
        {
            throw new InvalidOperationException(
                $"Cannot remove the last author ({author.Name} {author.Type}) from book {Id}");
        }

        if (!_authors.Remove(author))
        {
            throw new InvalidOperationException(
                $"Author {author.Name} {author.Type} not found in book {Id}");
        }
    }

    public void Rename(string newTitle) => this.Title = newTitle;

    public void Borrow()
    {
        if (Status == BookStatus.Borrowed)
        {
            throw new InvalidOperationException($"Book {Id} is already borrowed");
        }

        Status = BookStatus.Borrowed;
    }

    public void Return()
    {
        if (Status == BookStatus.Available)
        {
            throw new InvalidOperationException($"Book '{Id}' is not borrowed.");
        }

        Status = BookStatus.Available;
    }

    public static Book Create(
        string title,
        HashSet<Author> authors,
        DateOnly year,
        BookStatus status = BookStatus.Available)
    {
        ArgumentNullException.ThrowIfNull(authors);

        if (authors.Count == 0)
        {
            throw new InvalidOperationException("Authors cannot be empty");
        }

        return new Book(Guid.NewGuid(), title, authors, year, status);
    }

    public static Book Rehydrate(
        Guid id,
        string title,
        HashSet<Author> authors,
        DateOnly year,
        BookStatus status)
    {
        return new Book(id, title, authors, year, status);
    }
}