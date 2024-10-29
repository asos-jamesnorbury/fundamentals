using NSubstitute;
using NSubstitute.ReturnsExtensions;
using Xunit;

namespace Application.UnitTests;

public class PersistFileRequestHandlerShould
{
    [Fact]
    public void StoreResponse_GivenValidWebsite()
    {
        // First, we set up all the values which we know are going to be used in our test.
        // We create them as variables so we can use them as input, but also verify
        // the expected output.
        var url = "www.google.co.uk";
        var response = "content";

        // Next, we create the mocks which are passed into the class we're testing.
        // Using the NSubstitute framework allows us to set pre-defined responses
        // for different method calls. 
        var mockDownloader = Substitute.For<IWebsiteDownloader>();

        // As there is logic in the Handle method which relies on the response of
        // one of the dependencies, we need to mock the dependencies response.

        // In order to mock a method, we need to tell the NSubstitute framework which
        // method needs to be mocked. We do this using the Returns method.

        // The Returns method allows you to specify what is returned when the mocked
        // method is called. Providing a response allows you to test the rest of
        // your method after the mock is called. Different return values can
        // be provided to test different scenarios.
        mockDownloader.Get(url).Returns(response);
        var mockStorage = Substitute.For<IStorage>();
        var mockLogger = Substitute.For<ILogger>();

        var sut = new PersistWebsiteRequestHandler(mockDownloader, mockStorage, mockLogger);

        sut.Handle(url);

        // The Received method allows you to determine if your mock has been used in the way
        // you expected. This is useful to let you assert something has happened when
        // your method doesn't have a return value. In this case we are able to
        // assert the correct content was passed into the Save method.

        // While the Received method is good at determining if something has happened,
        // it can be restrictive if we do not have all the information within
        // the unit test. E.g. the path parameter used is in a private
        // variable. Arg.Any<string> allows us to tell NSubstitute we will
        // accept any string which is passed in as the path to
        // this method.
        mockStorage.Received(1).Save(response, Arg.Any<string>());
    }

    [Fact]
    public void ReturnEarly_GivenNothingDownloded()
    {
        var mockDownloader = Substitute.For<IWebsiteDownloader>();
        mockDownloader.Get("").ReturnsNull();
        var mockStorage = Substitute.For<IStorage>();
        var mockLogger = Substitute.For<ILogger>();
        var sut = new PersistWebsiteRequestHandler(mockDownloader, mockStorage, mockLogger);

        sut.Handle("");

        // As we are verifying that a method was not called, we aren't able to specify
        // an expected value for the Save method like we did before.
        // We are able to use the Arg.Any<> method to ensure that
        // this method was never called with any input.
        mockStorage.DidNotReceive().Save(Arg.Any<string>(), Arg.Any<string>());
    }

    [Fact]
    public void AppendGuidToOutputPathAsSubFolder_GivenContentDownloaded()
    {
        // We are following a TDD approach, so are writing the test before the implementation.
        var url = "www.google.co.uk";
        var response = "<html>Google</html>";
        var mockDownloader = Substitute.For<IWebsiteDownloader>();

        // We mock IWebsiteDownloader.Get() so we're able to reach the end of the method
        // to test the functionality we're going to implement.
        mockDownloader.Get(url).Returns(response);
        var mockStorage = Substitute.For<IStorage>();
        var mockLogger = Substitute.For<ILogger>();

        var sut = new PersistWebsiteRequestHandler(mockDownloader, mockStorage, mockLogger);

        sut.Handle(url);

        // Now we want to test something has happened again but we don't know exactly what the string will be.
        // We can't use the Arg.Any<> method here as there are some conditions on what is correct.
        // As a new GUID will be generated and we don't know what the value will be,
        // we can use the Arg.Is<> method to set a condition to allow a value
        // to be accepted if it meets our requirement.

        // If this is a simple statement it can all be nested in a single lambda, however
        // if there is more complicated logic it can be refactored to a separate
        // method which returns a bool to determine if our condition is met.
        mockStorage
            .Received(1)
            .Save(response, Arg.Is<string>(path => EndsWithGuid(path)));
    }

    [Fact]
    public void ThrowCustomException_WhenSavingResponse_GivenDirectoryNotFound()
    {
        // Again following a TDD approach, we want to ensure our application handles exceptions
        // correctly when one of the dependencies throws an exception.
        var response = "<html>Hello, world!</html>";
        var mockDownloader = Substitute.For<IWebsiteDownloader>();
        mockDownloader.Get(Arg.Any<string>()).Returns(response);
        var mockStorage = Substitute.For<IStorage>();
        var mockLogger = Substitute.For<ILogger>();

        // Like using Returns, we are also able to throw exceptions when the mock is called.
        mockStorage
            .When(x => x.Save(Arg.Any<string>(), Arg.Any<string>()))
            .Throw<DirectoryNotFoundException>();

        var sut = new PersistWebsiteRequestHandler(mockDownloader, mockStorage, mockLogger);

        // Using a Assert.Throws we're able to verify that the correct exception has been thrown.
        // This will catch our exception and verify it's the right type for us.
        // We must provide the code in a lambda so it can wrap it in
        // a try / catch block and assert the result.
        Assert.Throws<WebsiteStorageException>(() => sut.Handle("www.google.co.uk"));
    }

    // We can use the Theory attribute instead of Fact to provide multiple inputs to a test.
    [Theory]
    // Usually we're able to use InlineData to provide simple parameters. However, when
    // we want to pass classes as a parameter, xUnit requires us to use MemberData
    // or ClassData attributes to provide the parameters.
    [MemberData(nameof(ExceptionAndResponseData))]
    public void LogErrorMessage_WhenErrorThrownWhileSaving_GiveValidWebsite(Exception thrownException, string expectedMessage)
    {
        var mockDownloader = Substitute.For<IWebsiteDownloader>();
        mockDownloader.Get(Arg.Any<string>()).Returns("<html></html>");
        var mockStorage = Substitute.For<IStorage>();

        // We're able to setup the method to throw like before, except we're using the exception provided.
        mockStorage
            .When(x => x.Save(Arg.Any<string>(), Arg.Any<string>()))
            .Throws(thrownException);
        var mockLogger = Substitute.For<ILogger>();

        var sut = new PersistWebsiteRequestHandler(mockDownloader, mockStorage, mockLogger);

        Assert.Throws<WebsiteStorageException>(() => sut.Handle("url"));
        // Similarly, we're able to verify the expected outcome using the message provided.
        mockLogger.Received(1).Error(expectedMessage);
    }

    // To provide our test with instances of classes, we need to use the MemberData or ClassData attributes.
    // This lets us provide an array of arrays, which matches the parameters of our method.
    // The test will be ran for each element in the data array provided.
    public static IEnumerable<object[]> ExceptionAndResponseData => new List<object[]>
    {
        new object[] { new DirectoryNotFoundException(), "Output path does not exist." },
        new object[] { new InvalidOperationException(), "An unexpected error occurred." }
    };

    private static bool EndsWithGuid(string path)
    {
        var pathParts = path.Split(Path.DirectorySeparatorChar, StringSplitOptions.RemoveEmptyEntries);
        var guidString = pathParts.Last();

        // If guidString is a valid guid this returns true, otherwise it returns false.
        // We don't care about the output GUID so can use _ to discard it.
        return Guid.TryParse(guidString, out var _);
    }

}
