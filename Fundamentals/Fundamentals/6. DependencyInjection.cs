using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Fundamentals;

public class EntryPoint
{
    public void Run()
    {
        var builder = new ServiceCollection();
        builder.AddSingleton<ILogger, AppInsightsLogger>();
        builder.AddSingleton<IReturnRepository, ReturnRepository>();
        var serviceProvider = builder.BuildServiceProvider();
        var handler = serviceProvider.GetRequiredService<ReturnProcessHandler>();
    }
}

#region Application
public class ReturnProcessedHandler_
{
    public async Task HandleAsync(ReturnProcessedMessage message)
    {
        var logger = new ConsoleLogger();
        if (message.CreatedTime < DateTime.Now.AddDays(-14))
        {
            logger.LogInformation("Return outside of return window.");
            return;
        }

        var returnRepository = new ReturnRepository();
        await returnRepository.InsertAsync(message.CustomerReturn);
    }
}

public class ReturnProcessHandler
{
    private readonly IReturnRepository _returnRepository;
    private readonly ILogger _logger;

    public ReturnProcessHandler(IReturnRepository returnRepository, ILogger logger)
    {
        _returnRepository = returnRepository;
        _logger = logger;
    }

    public async Task HandleAsync(ReturnProcessedMessage message)
    {
        if (message.CreatedTime < DateTime.Now.AddDays(-14))
        {
            _logger.LogInformation("Return outside of return window.");
            return;
        }

        await _returnRepository.InsertAsync(message.CustomerReturn);
    }
}

#endregion

#region Infrastructure
public interface IReturnRepository
{
    Task InsertAsync(Return customerReturn);
}

public class ReturnRepository : IReturnRepository
{
    // Insert return into storage.
    public Task InsertAsync(Return customerReturn) => Task.CompletedTask;
}

public interface ILogger
{
    void LogInformation(string message);
}

public class ConsoleLogger : ILogger
{
    public void LogInformation(string message) => Console.WriteLine(message);
}

public class AppInsightsLogger : ILogger
{
    // Setup code to connect to Azure App Insights
    private readonly dynamic _appInsightsClient;

    public void LogInformation(string message)
    {
        _appInsightsClient.LogInformation(message);
    }
}
#endregion

#region Models
public record ReturnProcessedMessage(string MessageId, DateTime CreatedTime, Return CustomerReturn);
public record Return(string ReturnId, string OrderId, IEnumerable<Item> Items);
public record Item(string ItemId, string ItemName, string Sku);
#endregion
