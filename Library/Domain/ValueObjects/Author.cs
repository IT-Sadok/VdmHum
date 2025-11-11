namespace Domain.ValueObjects;

using Enums;
using Exceptions.Author;

public sealed record Author
{
    private const int MaxAuthorNameLength = 100;

    private Author(string? name, AuthorType type)
    {
        Name = name;
        Type = type;
    }

    public string? Name { get; }

    public AuthorType Type { get; }

    public static Author Known(string name) => new Author(NormalizeName(name), AuthorType.Known);

    public static Author Pseudonym(string pseudonym) => new Author(NormalizeName(pseudonym), AuthorType.Pseudonym);

    public static Author Anonymous() => new Author(null, AuthorType.Anonymous);

    public static Author Folk() => new Author(null, AuthorType.Folk);

    public static Author Unknown() => new Author(null, AuthorType.Unknown);

    private static string NormalizeName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new InvalidAuthorNameException("Author name cannot be empty");
        }

        name = name.Trim();

        if (name.Length > MaxAuthorNameLength)
        {
            throw new InvalidAuthorNameException($"Author name cannot be longer than {MaxAuthorNameLength} characters");
        }

        return name;
    }
}