using JetBrains.Annotations;

namespace AchiesUtilities.Extensions;

[PublicAPI]
public static class EnumExtension
{
    public static IEnumerable<T> GetDefinedValues<T>(this T e) where T : Enum
    {
        return Enum.GetValues(typeof(T)).Cast<T>();
    }

    public static IEnumerable<T> GetFlags<T>(this T e) where T : Enum
    {
        return GetDefinedValues(e).Where(en => e.HasFlag(en));
    }
}