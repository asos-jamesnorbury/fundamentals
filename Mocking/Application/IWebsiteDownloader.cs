namespace Application;

public interface IWebsiteDownloader
{
    Task<string> GetAsync(string url);
}
