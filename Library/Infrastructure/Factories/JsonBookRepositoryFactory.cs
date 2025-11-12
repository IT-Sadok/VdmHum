using Application.Interfaces;

namespace Infrastructure.Factories;

using System.Text.Json;
using Repositories;

public sealed class JsonBookRepositoryFactory(JsonSerializerOptions jsonOptions)
    : IBookRepositoryFactory
{
    public IBookRepository Create(string filePath) => new JsonBookRepository(filePath, jsonOptions);
}