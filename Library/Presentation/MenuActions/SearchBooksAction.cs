namespace Presentation.MenuActions;

using Application.Interfaces;
using Helpers;
using Interfaces;

public class SearchBooksAction(IBookService bookService) 
    : IMenuAction
{
    public string Key => "3";
    public string Description => "Search by author/title";

    public async Task ExecuteAsync()
    {
        Console.Write("Enter search query: ");
        var q = Console.ReadLine() ?? string.Empty;

        var results = await bookService.SearchAsync(q);
        MenuHelpers.PrintBooks(results);
    }
}