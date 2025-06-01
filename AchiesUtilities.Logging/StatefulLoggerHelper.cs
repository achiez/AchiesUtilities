namespace AchiesUtilities.Logging;

internal static class StatefulLoggerHelper
{
    public static IEnumerable<KeyValuePair<string, object?>> Merge(
        this IEnumerable<KeyValuePair<string, object?>> source,
        IEnumerable<KeyValuePair<string, object?>> second)
    {
        ArgumentNullException.ThrowIfNull(source);
        ArgumentNullException.ThrowIfNull(second);
        var properties = new Dictionary<string, object?>();
        foreach (var kvp in source)
        {
            properties[kvp.Key] = kvp.Value;
        }
        foreach (var kvp in second)
        {
            properties[kvp.Key] = kvp.Value;
        }
        return properties;
    }
}

public class LogProp
{
    public static KeyValuePair<string, object?> Create(string name, object? value)
    {
        return new KeyValuePair<string, object?>(name, value);
    }
}