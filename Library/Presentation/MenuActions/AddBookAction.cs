using Application.Contracts;

namespace Presentation.MenuActions;

using Application.Interfaces;
using Helpers;
using Interfaces;
using FluentValidation;

public class AddBookAction(
    IBookService bookService,
    IValidator<BookUpsertModel> validator,
    IBookInputService bookInput)
    : IMenuAction
{
    public string Key => "1";
    public string Description => "Add a new book";

    public async Task ExecuteAsync()
    {
        var model = bookInput.ReadBookFromConsole();

        var result = await validator.ValidateAsync(model);
        if (!result.IsValid)
        {
            MenuHelpers.PrintValidationErrors(result);
            return;
        }

        await bookService.AddBookAsync(model);

        MenuHelpers.Success("Book added successfully.");
    }
}