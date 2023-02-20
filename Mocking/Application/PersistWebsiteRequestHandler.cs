namespace Application;

public class PersistWebsiteRequestHandler
{
    private const string OUTPUT_PATH = "out";
    private readonly IWebsiteDownloader _downloader;
    private readonly IStorage _storage;

    public PersistWebsiteRequestHandler(IWebsiteDownloader downloader, IStorage storage)
    {
        _downloader = downloader;
        _storage = storage;
    }

    public void Handle(string url)
    {
        var response = _downloader.Get(url);
        if (response == null)
        {
            return;
        }

        _storage.Save(response, OUTPUT_PATH);
    }
}
