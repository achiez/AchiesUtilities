using JetBrains.Annotations;

namespace AchiesUtilities.Extensions;

[PublicAPI]
public static class ReaderWriterLockExtension
{
    public static void WriteLock(this ReaderWriterLockSlim rwLock, Action action)
    {
        rwLock.EnterWriteLock();
        try
        {
            action();
        }
        finally
        {
            rwLock.ExitWriteLock();
        }
    }

    public static T WriteLock<T>(this ReaderWriterLockSlim rwLock, Func<T> func)
    {
        rwLock.EnterWriteLock();
        try
        {
            return func();
        }
        finally
        {
            rwLock.ExitWriteLock();
        }
    }


    public static void ReadLock(this ReaderWriterLockSlim rwLock, Action action)
    {
        rwLock.EnterReadLock();
        try
        {
            action();
        }
        finally
        {
            rwLock.ExitReadLock();
        }
    }

    public static T ReadLock<T>(this ReaderWriterLockSlim rwLock, Func<T> func)
    {
        rwLock.EnterReadLock();
        try
        {
            return func();
        }
        finally
        {
            rwLock.ExitReadLock();
        }
    }
}