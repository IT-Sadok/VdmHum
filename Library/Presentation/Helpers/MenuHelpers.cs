namespace Presentation.Helpers;

using Application.Contracts;
using FluentValidation.Results;

public static class MenuHelpers
{
    public static Guid? ReadBookId(string operation)
    {
        Console.Write($"Enter book ID to {operation}: ");
        if (Guid.TryParse(Console.ReadLine(), out var id))
        {
            return id;
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("Invalid ID format.");
        Console.ResetColor();
        return null;
    }

    public static void PrintBooks(IEnumerable<BookResponseModel> books)
    {
        foreach (var b in books)
        {
            var authors = string.Join(", ", b.Authors.Select(a => a.Name ?? "N/A"));
            Console.WriteLine($"{b.Id} | {b.Title} | {authors} | {b.Year} | {b.Status}");
        }
    }

    public static void PrintValidationErrors(ValidationResult result)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("Validation failed:");
        foreach (var e in result.Errors)
        {
            Console.WriteLine($" - {e.ErrorMessage}");
        }

        Console.ResetColor();
    }
    
    public static void Warn(string m)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine(m);
        Console.ResetColor();
    }

    public static void Error(string m)
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine(m);
        Console.ResetColor();
    }

    public static void Success(string m)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine(m);
        Console.ResetColor();
    }

    public static void ContinuePrompt()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey(true);
    }
}