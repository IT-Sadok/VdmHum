namespace Application.Interfaces;

public interface IBookRepositoryProvider
{
    void Initialize(string filePath);

    IBookRepository GetRepository();
}