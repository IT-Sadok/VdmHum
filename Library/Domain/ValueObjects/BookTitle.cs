using Domain.Exceptions.BookTitle;

namespace Domain.ValueObjects;

public sealed record BookTitle
{
    public const int MaxTitleLength = 255;

    public string Value { get; }

    private BookTitle(string value) => this.Value = value;

    public static BookTitle Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidBookTitleException("Title cannot be empty");
        }

        value = value.Trim();

        if (value.Length > MaxTitleLength)
        {
            throw new InvalidBookTitleException($"Title cannot be longer than {MaxTitleLength} characters");
        }

        return new BookTitle(value);
    }
}