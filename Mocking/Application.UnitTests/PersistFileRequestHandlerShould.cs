using Moq;
using Xunit;

namespace Application.UnitTests;

public class PersistFileRequestHandlerShould
{
    [Fact]
    public async Task StoreResponse_GivenValidWebsite()
    {
        // First, we set up all the values which we know are going to be used in our test.
        // We create them as variables so we can use them as input, but also verify
        // the expected output.
        var url = "www.google.co.uk";
        var response = "content";

        // Next, we create the mocks which are passed into the class we're testing.
        // Using the Moq framework allows us to set pre-defined responses
        // for different method calls. 
        var mockDownloader = new Mock<IWebsiteDownloader>();

        // As there is logic in the HandleAsync method which relies on the response of
        // one of the dependencies, we need to mock the dependencies response.

        // In order to mock a method, we need to tell the Moq framework which
        // method needs to be mocked. We do this using the Setup method.

        // The Setup method uses a function in order to select what needs to be mocked.
        // This gives you control over when to mock a method. In this example,
        // we are mocking the GetAsync method on the IWebsiteDownloader,
        // only when the requestUrl is "www.google.co.uk".

        // The Returns method allows you to specify what is returned when the mocked
        // method is called. Providing a response allows you to test the rest of
        // your method after the mock is called. Different return values can
        // be provided to test different scenarios.
        mockDownloader.Setup(x => x.GetAsync(url)).ReturnsAsync(response);
        var mockStorage = new Mock<IStorage>();

        var sut = new PersistWebsiteRequestHandler(mockDownloader.Object, mockStorage.Object);

        await sut.HandleAsync(url);

        // The Verify method allows you to determine if your mock has been used in the way
        // you expected. This is useful to let you assert something has happend when
        // your method doesn't have a return value. In this case we are able to
        // assert the correct content was passed into the SaveAsync method.

        // While the Verify method is good at determining if something has happened,
        // it can be restrictive if we do not have all the information within
        // the unit test. E.g. the path parameter used is in a private
        // variable. It.IsAny<string> allows us to tell Moq we will
        // accept any string which is passed in as the path to
        // this method.
        mockStorage.Verify(x => x.SaveAsync(response, It.IsAny<string>()), Times.Once());
    }

    [Fact]
    public async Task ReturnEarly_GivenNothingDownloded()
    {
        var mockDownloader = new Mock<IWebsiteDownloader>();
        var storage = new Mock<IStorage>();
        var sut = new PersistWebsiteRequestHandler(mockDownloader.Object, storage.Object);

        await sut.HandleAsync("");

        // As we are verifying that a method was not called, we aren't able to specify
        // an expected value for the SaveAsync method like we did before.
        // We are able to use the It.IsAny<> method to ensure that
        // this method was never called with any input.
        storage.Verify(x => x.SaveAsync(It.IsAny<string>(), It.IsAny<string>()), Times.Never());
    }

    [Fact]
    public async Task AppendGuidToOutputPathAsSubFolder_GivenContentDownloaded()
    {
        // We are following a TDD approach, so are writing the test before the implementation.
        var url = "www.google.co.uk";
        var response = "<html>Google</html>";
        var mockDownloader = new Mock<IWebsiteDownloader>();

        // We mock IWebsiteDownloder.GetAsync() so we're able to reach the end of the method
        // to test the functionality we're going to implement.
        mockDownloader.Setup(x => x.GetAsync(url)).ReturnsAsync(response);
        var mockStorage = new Mock<IStorage>();

        var sut = new PersistWebsiteRequestHandler(mockDownloader.Object, mockStorage.Object);

        await sut.HandleAsync(url);

        // Now we want to test something has happened again but we don't know exactly what the string will be.
        // We can't use the It.IsAny<> method here as there are some conditions on what is correct.
        // As a new GUID will be generated and we don't know what the value will be,
        // we can use the It.Is<> method to set a condition to allow a value
        // to be accepted if it meets our requirement.

        // If this is a simple statement it can all be nested in a single lambda, however
        // if there is more complicated logic it can be refactored to a separate
        // method which returns a bool to determine if our condition is met.
        mockStorage.Verify(storage => storage.SaveAsync(response, It.Is<string>(path => EndsWithGuid(path))), Times.Once());
    }

    private static bool EndsWithGuid(string path)
    {
        var pathParts = path.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
        var guidString = pathParts.Last();

        // If guidString is a valid guid this returns true, otherwise it returns false.
        // We don't care about the output GUID so can use _ to discard it.
        return Guid.TryParse(guidString, out var _);
    }
}