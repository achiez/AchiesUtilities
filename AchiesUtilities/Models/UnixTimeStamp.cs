using System.Diagnostics;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace AchiesUtilities.Models;

public enum UnixFormat
{
    Seconds,
    Milliseconds,
    Microseconds,
    Ticks
}

[PublicAPI]
[Serializable]
[DebuggerDisplay("{Seconds}")]
public readonly struct UnixTimeStamp : IComparable<UnixTimeStamp>, IEquatable<UnixTimeStamp>
{
    private static DateTime Epoch => DateTime.UnixEpoch;
    public static readonly UnixTimeStamp Zero = FromDateTime(Epoch);

    [JsonIgnore] [Newtonsoft.Json.JsonIgnore]
    public readonly TimeSpan TimeSpan;

    [Newtonsoft.Json.JsonIgnore] public DateTime Time => ToUtcDateTime();
    public long Ticks => TimeSpan.Ticks;
    public long Seconds => ToLong();

    [UsedImplicitly]
    [JsonConstructor]
    [Newtonsoft.Json.JsonConstructor]
    private UnixTimeStamp(long ticks)
    {
        TimeSpan = new TimeSpan(ticks);
    }

    public UnixTimeStamp(TimeSpan timeSpan)
    {
        TimeSpan = timeSpan;
    }

    public UnixTimeStamp(long unix, UnixFormat format = UnixFormat.Seconds)
    {
        TimeSpan = format switch
        {
            UnixFormat.Seconds => TimeSpan.FromSeconds(unix),
            UnixFormat.Milliseconds => TimeSpan.FromMilliseconds(unix),
            UnixFormat.Microseconds => TimeSpan.FromMicroseconds(unix),
            UnixFormat.Ticks => new TimeSpan(unix),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
        };
    }

    #region ToLong

    public long ToLong(UnixFormat format = UnixFormat.Seconds)
    {
        return GetTimespanUnits(TimeSpan, format);
    }

    internal static long GetTimespanUnits(TimeSpan timeSpan, UnixFormat format)
    {
        return format switch
        {
            UnixFormat.Seconds => (long) timeSpan.TotalSeconds,
            UnixFormat.Milliseconds => (long) timeSpan.TotalMilliseconds,
            UnixFormat.Microseconds => (long) timeSpan.TotalMicroseconds,
            UnixFormat.Ticks => timeSpan.Ticks,
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
        };
    }

    #endregion

    #region ToDateTime

    public DateTime ToUtcDateTime()
    {
        return Epoch + TimeSpan;
    }

    public DateTime ToDateTime(TimeZoneInfo timeZoneInfo)
    {
        return TimeZoneInfo.ConvertTime(ToUtcDateTime(), timeZoneInfo);
    }

    public DateTime ToLocalDateTime()
    {
        return TimeZoneInfo.ConvertTime(ToUtcDateTime(), TimeZoneInfo.Local);
    }

    #endregion

    #region StaticDateTime

    public static DateTime ToDateTime(long timeStamp, UnixFormat format = UnixFormat.Seconds)
    {
        return Epoch + new UnixTimeStamp(timeStamp, format).TimeSpan;
    }

    public static DateTime ToDateTime(long timeStamp, TimeZoneInfo timeZoneInfo, UnixFormat format = UnixFormat.Seconds)
    {
        return TimeZoneInfo.ConvertTime(ToDateTime(timeStamp, format), timeZoneInfo);
    }

    #endregion

    /// <summary>
    ///     Converts DateTime to UTC format
    /// </summary>
    /// <param name="dateTime"></param>
    /// <param name="unixFormat">Format used for future equality</param>
    /// <returns>UTC UnixTimeStamp</returns>
    public static UnixTimeStamp FromDateTime(DateTime dateTime, UnixFormat unixFormat = UnixFormat.Seconds)
    {
        var timeSpan = dateTime.ToUniversalTime() - Epoch;
        return new UnixTimeStamp(GetTimespanUnits(timeSpan, unixFormat), unixFormat);
    }

    #region Object overrides

    public bool EqualsWithPrecision(UnixTimeStamp other, UnixFormat precision)
    {
        return TimeSpan.Equals(other.TimeSpan) ||
               GetTimespanUnits(TimeSpan, precision) == GetTimespanUnits(other.TimeSpan, precision);
    }

    public bool Equals(UnixTimeStamp other)
    {
        return TimeSpan.Equals(other.TimeSpan);
    }

    public override bool Equals(object? obj)
    {
        return obj is UnixTimeStamp other && Equals(other);
    }

    public override int GetHashCode()
    {
        return TimeSpan.GetHashCode() * 37;
    }

    public override string ToString()
    {
        return Seconds.ToString();
    }

    public int CompareTo(UnixTimeStamp other)
    {
        return TimeSpan.CompareTo(other.TimeSpan);
    }

    #endregion

    #region Operators

    #region Equality

    public static bool operator ==(UnixTimeStamp left, UnixTimeStamp right)
    {
        return left.Ticks == right.Ticks;
    }

    public static bool operator !=(UnixTimeStamp left, UnixTimeStamp right)
    {
        return !(left == right);
    }

    public static bool operator >(UnixTimeStamp left, UnixTimeStamp right)
    {
        return left.Ticks > right.Ticks;
    }

    public static bool operator <(UnixTimeStamp left, UnixTimeStamp right)
    {
        return left.Ticks < right.Ticks;
    }

    public static bool operator >=(UnixTimeStamp left, UnixTimeStamp right)
    {
        return left.Ticks >= right.Ticks;
    }

    public static bool operator <=(UnixTimeStamp left, UnixTimeStamp right)
    {
        return left.Ticks <= right.Ticks;
    }

    #endregion

    #region Unix&TimeSpan

    public static UnixTimeStamp operator +(UnixTimeStamp left, TimeSpan right)
    {
        return new UnixTimeStamp(left.TimeSpan + right);
    }

    public static UnixTimeStamp operator -(UnixTimeStamp left, TimeSpan right)
    {
        return new UnixTimeStamp(left.TimeSpan - right);
    }

    #endregion

    #region Unix&Unix

    public static TimeSpan operator -(UnixTimeStamp left, UnixTimeStamp right)
    {
        return left.TimeSpan - right.TimeSpan;
    }

    #endregion

    #region Unix&DateTime

    public static TimeSpan operator -(DateTime left, UnixTimeStamp right)
    {
        return left.ToUniversalTime() - right.ToUtcDateTime();
    }

    public static TimeSpan operator -(UnixTimeStamp left, DateTime right)
    {
        return left.ToUtcDateTime() - right.ToUniversalTime();
    }

    #endregion

    #endregion


    #region Parse

    public static UnixTimeStamp Parse(object obj, UnixFormat format = UnixFormat.Seconds)
    {
        ArgumentNullException.ThrowIfNull(obj);
        return obj switch
        {
            long l => new UnixTimeStamp(l, format),
            int i => new UnixTimeStamp(i, format),
            string s => Parse(s, format),
            _ => throw new FormatException($"Can't parse {nameof(UnixTimeStamp)} from value {obj}")
        };
    }

    public static UnixTimeStamp Parse(string s, UnixFormat format = UnixFormat.Seconds)
    {
        return new UnixTimeStamp(long.Parse(s), format);
    }

    public static bool TryParse(object? obj, out UnixTimeStamp unixTimeStamp, UnixFormat format = UnixFormat.Seconds)
    {
        unixTimeStamp = default;
        return obj switch
        {
            long l => TryParse(l, out unixTimeStamp, format),
            int i => TryParse(i, out unixTimeStamp, format),
            string s => TryParse(s, out unixTimeStamp, format),
            _ => false
        };
    }

    public static bool TryParse(string? s, out UnixTimeStamp unixTimeStamp, UnixFormat format = UnixFormat.Seconds)
    {
        if (long.TryParse(s, out var l))
        {
            unixTimeStamp = new UnixTimeStamp(l, format);
            return true;
        }

        unixTimeStamp = default;
        return false;
    }

    private static bool TryParse(long l, out UnixTimeStamp unixTimeStamp, UnixFormat format = UnixFormat.Seconds)
    {
        unixTimeStamp = new UnixTimeStamp(l, format);
        return true;
    }

    #endregion
}