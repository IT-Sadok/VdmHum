namespace Presentation.MenuActions;

using Application.Interfaces;
using Helpers;
using Interfaces;
using FluentValidation;
using Shared.Contracts;

public class AddBookAction(
    IBookService bookService,
    IValidator<BookUpsertDto> validator,
    IBookInputService bookInput)
    : IMenuAction
{
    public string Key => "1";
    public string Description => "Add a new book";

    public async Task ExecuteAsync()
    {
        var dto = bookInput.ReadBookFromConsole();

        var result = await validator.ValidateAsync(dto);
        if (!result.IsValid)
        {
            MenuHelpers.PrintValidationErrors(result);
            return;
        }

        await bookService.AddBookAsync(dto);

        MenuHelpers.Success("Book added successfully.");
    }
}