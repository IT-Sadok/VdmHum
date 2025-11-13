using System.Text;
using System.Text.Json;
using Application.Contracts;
using Application.Interfaces;
using Application.Services;
using Application.Validators;
using FluentValidation;
using Infrastructure.Factories;
using Infrastructure.FileStorage;
using Infrastructure.JsonConverters;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Presentation.Helpers;
using Presentation.Interfaces;
using Presentation.Menu;
using Presentation.MenuActions;
using Presentation.Services;

Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);

var builder = Host.CreateApplicationBuilder(args);

var jsonOptions = new JsonSerializerOptions
{
    WriteIndented = true,
    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
    Converters = { new AuthorJsonConverter() },
};

builder.Services
    .AddSingleton(jsonOptions)
    .AddSingleton<IFileStorage, JsonFileStorage>()
    .AddSingleton<IBookRepositoryFactory, JsonBookRepositoryFactory>()
    .AddValidatorsFromAssemblyContaining<BookUpsertModelValidator>()
    .AddScoped<Menu>()
    .AddScoped<IMenuAction, AddBookAction>()
    .AddScoped<IMenuAction, DeleteBookAction>()
    .AddScoped<IMenuAction, SearchBooksAction>()
    .AddScoped<IMenuAction, ShowAllBooksAction>()
    .AddScoped<IMenuAction, ShowAvailableBooksAction>()
    .AddScoped<IMenuAction, BorrowBookAction>()
    .AddScoped<IMenuAction, ReturnBookAction>()
    .AddScoped<IBookInputService, BookConsoleInputService>();

var app = builder.Build();

using var scope = app.Services.CreateScope();

var factory = scope.ServiceProvider.GetRequiredService<IBookRepositoryFactory>();
var filePath = await StartupHelpers.AskForJsonFileAsync();

var repository = factory.Create(filePath);
var bookService = new BookService(repository);

var actions = new IMenuAction[]
{
    new AddBookAction(bookService,
        scope.ServiceProvider.GetRequiredService<IValidator<BookUpsertModel>>(),
        scope.ServiceProvider.GetRequiredService<IBookInputService>()),

    new DeleteBookAction(bookService),
    new SearchBooksAction(bookService),
    new ShowAllBooksAction(bookService),
    new ShowAvailableBooksAction(bookService),
    new BorrowBookAction(bookService),
    new ReturnBookAction(bookService)
};

var menu = new Menu(actions);

try
{
    await menu.RunAsync();
}
catch (InvalidDataException ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine(ex.Message);
    Console.ResetColor();
}
catch (Exception ex)
{
    Console.ForegroundColor = ConsoleColor.Red;
    Console.WriteLine($"Unexpected error: {ex.Message}");
    Console.ResetColor();
}