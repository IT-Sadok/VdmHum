namespace Presentation.MenuActions;

using Application.Interfaces;
using Helpers;
using Interfaces;

public class ShowAllBooksAction(IBookService bookService)
    : IMenuAction
{
    public string Key => "4";
    public string Description => "Show all books in library";

    public async Task ExecuteAsync()
    {
        var books = await bookService.GetAllAsync();
        MenuHelpers.PrintBooks(books);
    }
}