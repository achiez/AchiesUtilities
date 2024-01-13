using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace AchiesUtilities.Extensions;

[PublicAPI]
public static class NumbersInStrings
{
   
    private static readonly Regex Regex = new("(\\d+)");

    /// <exception cref="FormatException"></exception>
    public static int ExtractNumeral(this string str)
    {
        if (TryExtractNumeral(str, out var result))
        {
            return result;
        }

        Throw(str);
        return default; //Never reached
    }

    /// <exception cref="FormatException"></exception>
    public static long ExtractNumeral64(this string str)
    {
        if (TryExtractNumeral64(str, out var result))
        {
            return result;
        }
        Throw(str);
        return default; //Never reached
    }

    /// <exception cref="FormatException"></exception>
    public static int ExtractFirstNumeral(this string str)
    {
        if (TryExtractFirstNumeral(str, out var result))
        {
            return result;
        }
        Throw(str);
        return default; //Never reached
    }

    /// <exception cref="FormatException"></exception>
    public static long ExtractFirstNumeral64(this string str)
    {
        if (TryExtractFirstNumeral64(str, out var result))
        {
            return result;
        }
        Throw(str);
        return default; //Never reached
    }


    public static bool TryExtractNumeral(this string str, out int result)
    {
        var chars = str.Where(char.IsDigit).ToArray();
        return int.TryParse(new string(chars), out result);
    }
    public static bool TryExtractNumeral64(this string str, out long result)
    {
        var chars = str.Where(char.IsDigit).ToArray();
        return long.TryParse(new string(chars), out result);
    }
    public static bool TryExtractFirstNumeral(this string str, out int result)
    {
        var match = Regex.Match(str);
        if (match.Success)
        {
            var val = match.Groups[1].Value;
            result = int.Parse(val);
            return true;
        }
        else
        {
            result = default;
            return false;
        }
    }
    public static bool TryExtractFirstNumeral64(this string str, out long result)
    {
        var match = Regex.Match(str);
        if (match.Success)
        {
            var val = match.Groups[1].Value;
            result = long.Parse(val);
            return true;
        }
        else
        {
            result = default;
            return false;
        }
    }

    [DoesNotReturn]
    private static void Throw(string str)
    {
        string msg;
        msg = str.Length > 100 ? "Provided string does not contain numerals. Provided string stored in Data" 
            : $"Provided string '{str}' does not contain numerals";

        throw new FormatException(msg)
        {
            Data = {{"ProvidedString", str}}
        };
    }
}