namespace Infrastructure.Providers;

using Application.Interfaces;
using Domain.Interfaces;

public sealed class JsonBookRepositoryProvider(IBookRepositoryFactory factory)
    : IBookRepositoryProvider
{
    private IBookRepository? _repo;

    public void Initialize(string filePath)
    {
        _repo = factory.Create(filePath);
    }

    public IBookRepository GetRepository()
    {
        if (_repo is null)
        {
            throw new InvalidOperationException("Repository not initialized. Call Initialize() first.");
        }

        return _repo;
    }
}