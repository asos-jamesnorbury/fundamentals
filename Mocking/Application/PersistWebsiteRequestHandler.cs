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

    public async Task HandleAsync(string url)
    {
        var response = await _downloader.GetAsync(url);
        if (response == null)
        {
            return;
        }

        await _storage.SaveAsync(response, OUTPUT_PATH);
    }
}
