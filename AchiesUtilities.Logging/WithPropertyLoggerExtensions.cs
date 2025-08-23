using AchiesUtilities.Models;
using Microsoft.Extensions.Logging;

namespace AchiesUtilities.Logging;

public static class WithPropertyLoggerExtensions
{
    public static ILogger WithProperty(this ILogger logger, string name, object? value)
    {
        ArgumentNullException.ThrowIfNull(logger);
        return new StatefulLogger(logger, [KeyValuePair.Create(name, value)]);
    }

    public static ILogger<T> WithProperty<T>(this ILogger<T> logger, string name, object? value)
    {
        ArgumentNullException.ThrowIfNull(logger);
        return new StatefulLogger<T>(logger, [KeyValuePair.Create(name, value)]);
    }

    public static ILogger WithProperty(this ILogger logger, KeyValuePair<string, object?> property)
    {
        ArgumentNullException.ThrowIfNull(logger);
        return new StatefulLogger(logger, [property]);
    }

    public static ILogger WithProperties(this ILogger logger, IEnumerable<KeyValuePair<string, object?>> properties)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(properties);

        return new StatefulLogger(logger, properties);
    }

    public static ILogger WithProperties(this ILogger logger, params (string key, object? value)[] properties)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(properties);

        var array = new KeyValuePair<string, object?>[properties.Length];
        for (var i = 0; i < properties.Length; i++)
        {
            var (key, value) = properties[i];
            array[i] = new KeyValuePair<string, object?>(key, value);
        }

        return new StatefulLogger(logger, array);
    }

    public static ILogger WithProperty<TProperty>(this ILogger logger, PropertyKey<TProperty> key, TProperty value)
    {
        ArgumentNullException.ThrowIfNull(logger);
        return logger.WithProperty(KeyValuePair.Create<string, object?>(key.Name, value));
    }

    public static ILogger<T> WithProperty<T, TProperty>(this ILogger<T> logger, PropertyKey<TProperty> key,
        TProperty value)
    {
        ArgumentNullException.ThrowIfNull(logger);
        return logger.WithProperty<T>(key.Name, value);
    }

    public static ILogger<T> WithProperties<T>(this ILogger<T> logger,
        IEnumerable<KeyValuePair<string, object?>> properties)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(properties);
        return new StatefulLogger<T>(logger, properties);
    }

    public static ILogger<T> WithProperties<T>(this ILogger<T> logger, params (string key, object? value)[] properties)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(properties);
        var array = new KeyValuePair<string, object?>[properties.Length];
        for (var i = 0; i < properties.Length; i++)
        {
            var (key, value) = properties[i];
            array[i] = new KeyValuePair<string, object?>(key, value);
        }

        return new StatefulLogger<T>(logger, array);
    }

    public static IEnumerable<KeyValuePair<string, object?>> GetProperties(this ILogger logger)
    {
        ArgumentNullException.ThrowIfNull(logger);
        if (logger is StatefulLogger sl)
        {
            return sl.ToList();
        }

        return [];
    }
}