namespace Domain.ValueObjects;

using Enums;
using Exceptions.Author;

public sealed record Author
{
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
            throw new ArgumentException("Author name cannot be empty");
        }

        name = name.Trim();
        
        return name;
    }
}