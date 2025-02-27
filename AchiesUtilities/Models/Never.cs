using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace AchiesUtilities.Models;

[PublicAPI]
public readonly struct Never<T>
{
    public static T Returns => Throw();

    public static implicit operator T(Never<T> _) => Throw();

    private static T Throw() => throw new NeverReturnException();
}

public class NeverReturnException : Exception
{
    public NeverReturnException()
        : base("This code should never be reached. The source code is invalid.")
    {
    }

    public NeverReturnException(string message)
        : base(message)
    {
    }

    public NeverReturnException(string message, Exception innerException)
        : base(message, innerException)
    {
    }
}
