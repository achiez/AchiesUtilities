using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace AchiesUtilities.Helpers;

[PublicAPI]
public static class ArgumentOutOfRangeHelper
{
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void ThrowIfOutOfRangeExclusive(decimal value, decimal min, decimal max,
        [CallerArgumentExpression(nameof(value))] string paramName = "")
    {
        if (value <= min || value >= max)
        {
            throw new ArgumentOutOfRangeException(paramName, value, $"Value must be between {min} and {max}");
        }
    }

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void ThrowIfOutOfRange(decimal value, decimal min, decimal max,
        [CallerArgumentExpression(nameof(value))] string paramName = "")
    {
        if (value < min || value > max)
        {
            throw new ArgumentOutOfRangeException(paramName, value,
                $"Value must be between {min} and {max} or equal to bounds");
        }
    }

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void ThrowIfOutOfRangeExclusiveRight(decimal value, decimal min, decimal max,
        [CallerArgumentExpression(nameof(value))] string paramName = "")
    {
        if (value < min || value >= max)
        {
            throw new ArgumentOutOfRangeException(paramName, value,
                $"Value must be between {min} and {max} or equal {min}");
        }
    }

    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static void ThrowIfOutOfRangeExclusiveLeft(decimal value, decimal min, decimal max,
        [CallerArgumentExpression(nameof(value))] string paramName = "")
    {
        if (value <= min || value > max)
        {
            throw new ArgumentOutOfRangeException(paramName, value,
                $"Value must be between {min} and {max} or equal {max}");
        }
    }
}