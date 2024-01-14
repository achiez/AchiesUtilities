using JetBrains.Annotations;
using System;

namespace AchiesUtilities.Extensions;

[PublicAPI]
public static class ExceptionExtension
{
    public static TException? ExtractException<TException>(this Exception ex) where TException : Exception
    {
        while (true)
        {
            if (ex is TException tEx) return tEx;
            if (ex.InnerException == null) return null;
            ex = ex.InnerException;
        }
    }
}