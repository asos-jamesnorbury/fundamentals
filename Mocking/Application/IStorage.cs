namespace Application;

public interface IStorage
{
    Task SaveAsync(string content, string path);
}