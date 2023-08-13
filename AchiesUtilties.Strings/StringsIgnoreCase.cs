using JetBrains.Annotations;

namespace AchiesUtilities.Strings;

[PublicAPI]
public static class StringsIgnoreCase
{
    public static bool EqualsIgnoreCase(this string str, string value)
        => str.Equals(value, StringComparison.InvariantCultureIgnoreCase);

    public static bool ContainsIgnoreCase(this string str, string value)
        => str.Contains(value, StringComparison.InvariantCultureIgnoreCase);

    public static bool StartsWithIgnoreCase(this string str, string value)
        => str.StartsWith(value, StringComparison.InvariantCultureIgnoreCase);

}