using System.Text;
using System.Text.Json;
using Application.Interfaces;
using Application.Services;
using Application.Validators;
using FluentValidation;
using Infrastructure.Factories;
using Infrastructure.JsonConverters;
using Infrastructure.Providers;
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
    .AddSingleton<IBookRepositoryFactory, JsonBookRepositoryFactory>()
    .AddSingleton<IBookRepositoryProvider, JsonBookRepositoryProvider>();

builder.Services.AddScoped<IBookService>(sp =>
{
    var provider = sp.GetRequiredService<IBookRepositoryProvider>();
    return new BookService(provider.GetRepository());
});

builder.Services.AddValidatorsFromAssemblyContaining<BookUpsertDtoValidator>();

builder.Services
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

var provider = scope.ServiceProvider.GetRequiredService<IBookRepositoryProvider>();
var filePath = await StartupHelpers.AskForJsonFileAsync();
provider.Initialize(filePath);

var menu = scope.ServiceProvider.GetRequiredService<Menu>();

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