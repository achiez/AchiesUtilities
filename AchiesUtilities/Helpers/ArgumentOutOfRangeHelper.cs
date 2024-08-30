using System.Runtime.CompilerServices;

namespace AchiesUtilities.Helpers;

public static class ArgumentOutOfRangeHelper
{
    public static void ThrowIfOutOfRangeExclusive(decimal value, decimal min, decimal max, [CallerArgumentExpression(nameof(value))] string paramName = "")
    {
        if (value <= min || value >= max)
        {
            throw new ArgumentOutOfRangeException(paramName, value, $"Value must be between {min} and {max}");
        }
    }

    public static void ThrowIfOutOfRange(decimal value, decimal min, decimal max, [CallerArgumentExpression(nameof(value))] string paramName = "")
    {
        if (value < min || value > max)
        {
            throw new ArgumentOutOfRangeException(paramName, value, $"Value must be between {min} and {max} or equal to bounds");
        }

    }

    public static void ThrowIfOutOfRangeExclusiveRight(decimal value, decimal min, decimal max, [CallerArgumentExpression(nameof(value))] string paramName = "")
    {
        if (value < min || value >= max)
        {
            throw new ArgumentOutOfRangeException(paramName, value, $"Value must be between {min} and {max} or equal {min}");
        }
    }

    public static void ThrowIfOutOfRangeExclusiveLeft(decimal value, decimal min, decimal max, [CallerArgumentExpression(nameof(value))] string paramName = "")
    {
        if (value <= min || value > max)
        {
            throw new ArgumentOutOfRangeException(paramName, value, $"Value must be between {min} and {max} or equal {max}");
        }
    }

}