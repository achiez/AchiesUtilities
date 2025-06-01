using Microsoft.Extensions.Logging;

namespace AchiesUtilities.Logging;

public class LogFactory : ILogFactory
{
    private readonly ILoggerFactory _loggerFactory;

    public LogFactory(ILoggerFactory loggerFactory)
    {
        _loggerFactory = loggerFactory;
    }

    public void Dispose()
    {
        _loggerFactory.Dispose();
    }

    public void AddProvider(ILoggerProvider provider)
    {
        _loggerFactory.AddProvider(provider);
    }

    public ILogger CreateLogger(string categoryName)
    {
        return _loggerFactory.CreateLogger(categoryName);
    }


    public ILogger<T> WithProperty<T>(ILogger<T> logger, string name, object? property)
    {
        ArgumentNullException.ThrowIfNull(logger);

        return logger.WithProperty(name, property);
    }

    public ILogger WithProperty<T>(ILogger logger, string name, T property)
    {
        ArgumentNullException.ThrowIfNull(logger);
        return logger.WithProperty(name, property);
    }

    public ILogger<T> WithProperties<T>(ILogger<T> logger, IEnumerable<KeyValuePair<string, object?>> properties)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(properties);
        return new StatefulLogger<T>(logger, properties);
    }
}