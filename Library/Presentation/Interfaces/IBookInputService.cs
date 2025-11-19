using Application.Contracts;

namespace Presentation.Interfaces;

public interface IBookInputService
{
    BookUpsertModel ReadBookFromConsole();
}