using System.Runtime.Serialization;

namespace Application;
public class WebsiteStorageException : Exception
{
    public WebsiteStorageException()
    {
    }

    public WebsiteStorageException(string? message) : base(message)
    {
    }

    public WebsiteStorageException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    protected WebsiteStorageException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
