namespace Presentation.MenuActions;

using Application.Interfaces;
using Helpers;
using Interfaces;

public class ReturnBookAction(IBookService bookService) 
    : IMenuAction
{
    public string Key => "7";
    public string Description => "Return a book";

    public async Task ExecuteAsync()
    {
        var id = MenuHelpers.ReadBookId("return");
        if (id is null) return;

        try
        {
            await bookService.ReturnBookAsync(id.Value);
            MenuHelpers.Success("Book returned successfully.");
        }
        catch (Exception ex)
        {
            MenuHelpers.Error(ex.Message);
        }
    }
}