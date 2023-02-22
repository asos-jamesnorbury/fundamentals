namespace Application;

public class PersistWebsiteRequestHandler
{
    private const string OUTPUT_PATH = "out";
    private readonly IWebsiteDownloader _downloader;
    private readonly IStorage _storage;
    private readonly ILogger _log;

    public PersistWebsiteRequestHandler(IWebsiteDownloader downloader, IStorage storage, ILogger log)
    {
        _downloader = downloader;
        _storage = storage;
        _log = log;
    }

    public void Handle(string url)
    {
        var response = _downloader.Get(url);
        if (response == null)
        {
            return;
        }

        try
        {
            var path = Path.Combine(OUTPUT_PATH, Guid.NewGuid().ToString());
            _storage.Save(response, path);
        }
        catch (DirectoryNotFoundException ex)
        {
            var message = "Output path does not exist.";
            _log.Error(message);
            throw new WebsiteStorageException(message, ex);
        }
        catch (InvalidOperationException ex)
        {
            var message = "An unexpected error occurred.";
            _log.Error(message);
            throw new WebsiteStorageException(message, ex);
        }
    }
}
