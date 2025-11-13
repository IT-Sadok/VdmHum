using Shared.Contracts;

namespace Presentation.Interfaces;

public interface IBookInputService
{
    BookUpsertModel ReadBookFromConsole();
}