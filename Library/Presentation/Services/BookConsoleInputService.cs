namespace Presentation.Services;

using Domain.Enums;
using Domain.ValueObjects;
using Helpers;
using Interfaces;
using Shared.Contracts;

public class BookConsoleInputService : IBookInputService
{
    public BookUpsertDto ReadBookFromConsole()
    {
        string title = ReadTitle();
        int year = ReadYear();
        bool isBc = ReadIsBcFlag();
        List<AuthorDto> authors = ReadAuthors();

        return new BookUpsertDto(title, authors, year, isBc, BookStatus.Available);
    }

    private static string ReadTitle()
    {
        Console.Write("Enter title: ");
        return Console.ReadLine()?.Trim() ?? string.Empty;
    }

    private static int ReadYear()
    {
        Console.Write($"Enter publication year ({HistoricalYear.MinYearValue}–{HistoricalYear.MaxYearValue}): ");
        int.TryParse(Console.ReadLine(), out var year);
        return year;
    }

    private static bool ReadIsBcFlag()
    {
        while (true)
        {
            Console.WriteLine();
            Console.WriteLine("Select era: 1 = BC, 2 = AD (or just press enter for AD)");
            Console.Write("Enter number (1/2): ");
            var input = Console.ReadLine()?.Trim();

            if (input == "1")
            {
                return true;
            }

            if (input == "2" || string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            MenuHelpers.Warn("Invalid input. Please enter 1 or 2.");
        }
    }

    private static List<AuthorDto> ReadAuthors()
    {
        var authors = new List<AuthorDto>();
        Console.WriteLine("Now let's add authors (leave blank to finish):");

        while (true)
        {
            var author = ReadAuthor();
            if (author is null)
            {
                break;
            }

            authors.Add(author);
            MenuHelpers.Success("Author added.");
        }

        return authors;
    }

    private static AuthorDto? ReadAuthor()
    {
        while (true)
        {
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("Choose author type:");
            Console.ResetColor();

            foreach (var value in Enum.GetValues<AuthorType>())
            {
                Console.WriteLine($"{(int)value} - {value}");
            }

            Console.Write("Enter number (or press Enter to finish): ");
            var typeInput = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(typeInput))
            {
                return null;
            }

            if (!int.TryParse(typeInput, out var typeInt) || !Enum.IsDefined(typeof(AuthorType), typeInt))
            {
                MenuHelpers.Warn("Invalid option. Try again.");
                continue;
            }

            var authorType = (AuthorType)typeInt;
            string? name = null;

            if (authorType is not (AuthorType.Known or AuthorType.Pseudonym))
            {
                return new AuthorDto(name, authorType);
            }

            Console.Write("Enter author name: ");
            name = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(name))
            {
                return new AuthorDto(name, authorType);
            }

            MenuHelpers.Warn("Name cannot be empty for this author type.");
        }
    }
}