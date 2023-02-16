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
        var requestUrl = "www.google.co.uk";
        var response = "content";
        var request = new PersistWebsiteRequest { Url = requestUrl };

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
        mockDownloader.Setup(x => x.GetAsync(requestUrl)).ReturnsAsync(response);
        var mockStorage = new Mock<IStorage>();

        var sut = new PersistWebsiteRequestHandler(mockDownloader.Object, mockStorage.Object);

        await sut.HandleAsync(request);

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
}