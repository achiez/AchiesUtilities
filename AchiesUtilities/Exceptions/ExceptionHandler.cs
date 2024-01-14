using JetBrains.Annotations;

namespace AchiesUtilities.Exceptions;
[PublicAPI]
public static class ExceptionHandler
{
    public static void Ignore(Action a)
    {
        try { a(); } catch { }
    }

    public static T? Ignore<T>(Func<T> a)
    {
        try { return a(); } catch { return default; }
    }

    public static async Task Ignore(Func<Task> a)
    {
        try { await a(); } catch{}
    }

    public static async Task<T?> Ignore<T>(Func<Task<T>> a)
    {
        try { return await a(); } catch { return default; }
    }
}