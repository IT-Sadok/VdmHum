namespace Application.Interfaces;

using Domain.Interfaces;

public interface IBookRepositoryProvider
{
    void Initialize(string filePath);

    IBookRepository GetRepository();
}