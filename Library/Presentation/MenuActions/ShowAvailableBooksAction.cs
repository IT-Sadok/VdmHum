namespace Presentation.MenuActions;

using Application.Interfaces;
using Helpers;
using Interfaces;

public class ShowAvailableBooksAction(IBookService bookService)
    : IMenuAction
{
    public string Key => "5";
    public string Description => "Show all available books";

    public async Task ExecuteAsync()
    {
        var books = await bookService.GetAllAvailableAsync();
        MenuHelpers.PrintBooks(books);
    }
}