using JetBrains.Annotations;

namespace AchiesUtilities.Exceptions;

[PublicAPI]
public static class ExceptionHandler
{
    public static void Ignore(Action a)
    {
        try
        {
            a();
        }
        catch
        {
        }
    }

    public static T? Ignore<T>(Func<T> a)
    {
        try
        {
            return a();
        }
        catch
        {
            return default;
        }
    }

    public static async Task Ignore(Func<Task> a)
    {
        try
        {
            await a();
        }
        catch
        {
        }
    }

    public static async Task<T?> Ignore<T>(Func<Task<T>> a)
    {
        try
        {
            return await a();
        }
        catch
        {
            return default;
        }
    }


    public static void Ignore<TException>(Action a) where TException : Exception
    {
        try
        {
            a();
        }
        catch (TException)
        {
        }
    }

    public static T? Ignore<T, TException>(Func<T> a) where TException : Exception
    {
        try
        {
            return a();
        }
        catch (Exception)
        {
            return default;
        }
    }

    public static async Task Ignore<TException>(Func<Task> a) where TException : Exception
    {
        try
        {
            await a();
        }
        catch (TException)
        {
        }
    }

    public static async Task<T?> Ignore<T, TException>(Func<Task<T>> a) where TException : Exception
    {
        try
        {
            return await a();
        }
        catch (TException)
        {
            return default;
        }
    }
}