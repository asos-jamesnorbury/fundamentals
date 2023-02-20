namespace Application;

public interface IStorage
{
    void Save(string content, string path);
}