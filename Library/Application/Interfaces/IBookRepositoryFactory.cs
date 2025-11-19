namespace Application.Interfaces;

public interface IBookRepositoryFactory
{
    IBookRepository Create(string filePath);
}