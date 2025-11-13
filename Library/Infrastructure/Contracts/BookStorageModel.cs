namespace Infrastructure.Contracts;

using Domain.Enums;

public record BookStorageModel(
    Guid Id, 
    string Title, 
    List<AuthorStorageModel> Authors, 
    int Year, 
    BookStatus Status);