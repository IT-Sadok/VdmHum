namespace Presentation.MenuActions;

using Application.Interfaces;
using Helpers;
using Interfaces;

public class DeleteBookAction(IBookService bookService) 
    : IMenuAction
{
    public string Key => "2";
    public string Description => "Delete a book";

    public async Task ExecuteAsync()
    {
        var id = MenuHelpers.ReadBookId("delete");
        if (id is null)
        {
            return;
        }

        try
        {
            await bookService.DeleteBookAsync(id.Value);
            MenuHelpers.Success("Book deleted.");
        }
        catch (Exception ex)
        {
            MenuHelpers.Error(ex.Message);
        }
    }
}