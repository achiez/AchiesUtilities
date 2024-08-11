using JetBrains.Annotations;

namespace AchiesUtilities.Extensions;

[PublicAPI]
public static class StringsIgnoreCase
{

    /// <summary>
    /// Equivalent to <see cref="string.Equals(string, string, StringComparison)"/> with <see cref="StringComparison.OrdinalIgnoreCase"/>
    /// </summary>
    /// <param name="str"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool EqualsIgnoreCase(this string str, string value)
        => str.Equals(value, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Equivalent to <see cref="string.Contains(string, StringComparison)"/> with <see cref="StringComparison.OrdinalIgnoreCase"/>
    /// </summary>
    /// <param name="str"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool ContainsIgnoreCase(this string str, string value)
        => str.Contains(value, StringComparison.OrdinalIgnoreCase);


    /// <summary>
    /// Equivalent to <see cref="string.StartsWith(string, StringComparison)"/> with <see cref="StringComparison.OrdinalIgnoreCase"/>
    /// </summary>
    /// <param name="str"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool StartsWithIgnoreCase(this string str, string value)
        => str.StartsWith(value, StringComparison.OrdinalIgnoreCase);


    /// <summary>
    /// Equivalent to <see cref="string.EndsWith(string, StringComparison)"/> with <see cref="StringComparison.OrdinalIgnoreCase"/>
    /// </summary>
    /// <param name="str"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static bool EndsWithIgnoreCase(this string str, string value)
        => str.EndsWith(value, StringComparison.OrdinalIgnoreCase);

    /// <summary>
    /// Equivalent to <see cref="string.Replace(string, string, StringComparison)"/> with <see cref="StringComparison.OrdinalIgnoreCase"/>
    /// </summary>
    /// <param name="str"></param>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    /// <returns></returns>
    public static string ReplaceIgnoreCase(this string str, string oldValue, string newValue)
        => str.Replace(oldValue, newValue, StringComparison.OrdinalIgnoreCase);
}