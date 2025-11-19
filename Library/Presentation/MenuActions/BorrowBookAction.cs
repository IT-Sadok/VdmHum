namespace Presentation.MenuActions;

using Application.Interfaces;
using Helpers;
using Interfaces;

public class BorrowBookAction(IBookService bookService)
    : IMenuAction
{
    public string Key => "6";
    public string Description => "Borrow a book";

    public async Task ExecuteAsync()
    {
        var id = MenuHelpers.ReadBookId("borrow");
        if (id is null) return;

        try
        {
            await bookService.BorrowBookAsync(id.Value);
            MenuHelpers.Success("Book borrowed successfully.");
        }
        catch (Exception ex)
        {
            MenuHelpers.Error(ex.Message);
        }
    }
}