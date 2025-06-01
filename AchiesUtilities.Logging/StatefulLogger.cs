using System.Collections;
using Microsoft.Extensions.Logging;

namespace AchiesUtilities.Logging;

internal class StatefulLogger : ILogger, IEnumerable<KeyValuePair<string, object?>>
{
    private readonly ILogger _logger;
    private readonly Dictionary<string, object?> _values;
    
    public StatefulLogger(ILogger logger, IEnumerable<KeyValuePair<string, object?>> values)
    {
       
        _values = values.ToDictionary();
        if (logger is StatefulLogger sl)
        {
            _values = sl._values.Merge(_values).ToDictionary(StringComparer.OrdinalIgnoreCase);
            _logger = sl._logger;
        }
        else
        {
            _logger = logger;
        }
    }

    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception,
        Func<TState, Exception?, string> formatter)
    {
        if (state is IEnumerable<KeyValuePair<string, object?>> logValues)
        {
            var mergedState = _values.Merge(logValues);
            _logger.Log(logLevel, eventId, mergedState, exception, Formatter);
        }
        else
        {
            _logger.Log(logLevel, eventId, state, exception, formatter);
        }

        string Formatter(IEnumerable<KeyValuePair<string, object?>> x, Exception? e)
        {
            return formatter(state, exception);
        }
    }

    public bool IsEnabled(LogLevel logLevel)
    {
        return _logger.IsEnabled(logLevel);
    }

    public IDisposable? BeginScope<TState>(TState state) where TState : notnull
    {
        return _logger.BeginScope(state);
    }

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        return _values.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

internal class StatefulLogger<T> : StatefulLogger, ILogger<T>
{
    public StatefulLogger(ILogger<T> logger, IEnumerable<KeyValuePair<string, object?>> values)
        : base(logger, values)
    {
    }
}