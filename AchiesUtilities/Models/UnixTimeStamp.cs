using JetBrains.Annotations;
using Newtonsoft.Json;
using System.Diagnostics;

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
public readonly struct UnixTimeStamp //TODO: Implicit Conversion, TryParse 
{
    private static DateTime Epoch => DateTime.UnixEpoch;
    public static readonly UnixTimeStamp Zero = FromDateTime(Epoch);

    [System.Text.Json.Serialization.JsonIgnore]
    [JsonIgnore] public readonly TimeSpan TimeSpan;
    [JsonIgnore] public DateTime Time => ToUtcDateTime();
    public long Ticks => TimeSpan.Ticks;
    public long Seconds => ToLong();

    [System.Text.Json.Serialization.JsonConstructor]
    [JsonConstructor]
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
            UnixFormat.Microseconds => TimeSpan.FromMilliseconds(unix) * 1000,
            UnixFormat.Ticks => new TimeSpan(unix),
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
        };
    }

    /// <summary>
    /// Used only if UnixTimeStamp represented in local timezone offset
    /// <br/>Even if it is weird usage, some services can provide offset timestamp.
    /// </summary>
    /// <param name="unix"></param>
    /// <param name="format"></param>
    /// <param name="unixTimeZone"></param>
    public UnixTimeStamp(long unix, UnixFormat format, TimeZoneInfo unixTimeZone)
        : this(unix + unixTimeZone.BaseUtcOffset.Ticks, format)
    { }

    #region GetLong

    public long ToLong(UnixFormat format = UnixFormat.Seconds) => GetTimespanUnits(TimeSpan, format);
    internal static long GetTimespanUnits(TimeSpan timeSpan, UnixFormat format)
    {
        return format switch
        {
            UnixFormat.Seconds => (long)timeSpan.TotalSeconds,
            UnixFormat.Milliseconds => (long)timeSpan.TotalMilliseconds,
            UnixFormat.Microseconds => (long)timeSpan.TotalMilliseconds * 1000,
            UnixFormat.Ticks => timeSpan.Ticks,
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
        };
    }


    #endregion

    #region GetDateTime
    public DateTime ToUtcDateTime() => Epoch + TimeSpan;
    public DateTime ToDateTime(TimeZoneInfo timeZoneInfo) => TimeZoneInfo.ConvertTime(ToUtcDateTime(), timeZoneInfo);
    public DateTime ToLocalDateTime() => TimeZoneInfo.ConvertTime(ToUtcDateTime(), TimeZoneInfo.Local);

    #endregion

    #region StaticDateTime
    public static DateTime ToDateTime(long timeStamp, UnixFormat format = UnixFormat.Seconds)
        => Epoch + new UnixTimeStamp(timeStamp, format).TimeSpan;

    public static DateTime ToDateTime(long timeStamp, TimeZoneInfo timeZoneInfo, UnixFormat format = UnixFormat.Seconds)
        => TimeZoneInfo.ConvertTime(ToDateTime(timeStamp, format), timeZoneInfo);

    #endregion

    /// <summary>
    /// Converts DateTime to UTC format
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
    public bool Equals(UnixTimeStamp obj, UnixFormat format = UnixFormat.Seconds)
    {
        return obj.TimeSpan.Equals(TimeSpan) || GetTimespanUnits(TimeSpan, format) == GetTimespanUnits(obj.TimeSpan, format);
    }

    public override bool Equals(object? obj)
    {
        return obj is UnixTimeStamp other && Equals(other);
    }
    public override int GetHashCode()
    {
        return TimeSpan.GetHashCode();
    }
    public override string ToString()
    {
        return Seconds.ToString();
    }
    #endregion

    #region Operators

    #region Equality
    public static bool operator ==(UnixTimeStamp left, UnixTimeStamp right) => left.Ticks == right.Ticks;
    public static bool operator !=(UnixTimeStamp left, UnixTimeStamp right) => !(left == right);
    public static bool operator >(UnixTimeStamp left, UnixTimeStamp right) => left.Ticks > right.Ticks;
    public static bool operator <(UnixTimeStamp left, UnixTimeStamp right) => left.Ticks < right.Ticks;
    public static bool operator >=(UnixTimeStamp left, UnixTimeStamp right) => left.Ticks >= right.Ticks;
    public static bool operator <=(UnixTimeStamp left, UnixTimeStamp right) => left.Ticks <= right.Ticks;

    #endregion

    #region Unix&TimeSpan

    public static UnixTimeStamp operator +(UnixTimeStamp left, TimeSpan right) => new(left.TimeSpan + right);
    public static UnixTimeStamp operator -(UnixTimeStamp left, TimeSpan right) => new(left.TimeSpan - right);

    #endregion

    #region Unix&Unix

    public static TimeSpan operator -(UnixTimeStamp left, UnixTimeStamp right) => left.TimeSpan - right.TimeSpan;

    #endregion

    #region Unix&DateTime
    public static TimeSpan operator -(DateTime left, UnixTimeStamp right) => left.ToUniversalTime() - right.ToUtcDateTime();
    public static TimeSpan operator -(UnixTimeStamp left, DateTime right) => left.ToUtcDateTime() - right.ToUniversalTime();


    #endregion


    #endregion


    #region Parse

    public static UnixTimeStamp Parse(object obj, UnixFormat format = UnixFormat.Seconds)
    {
        if (obj == null!)
        {
            throw new ArgumentNullException(nameof(obj));
        }
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

    #endregion
}
