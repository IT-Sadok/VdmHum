namespace Presentation.Interfaces;

public interface IMenuAction
{
    string Key { get; }
    string Description { get; }
    Task ExecuteAsync();
}