using System.Globalization;
using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace AchiesUtilities;

public enum UnixFormat
{
    Seconds,
    Milliseconds,
    Microseconds,
    Nanoseconds
}

[PublicAPI]
public struct UnixTimeStamp
{
    private static readonly DateTime Epoch = new(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
    public static readonly UnixTimeStamp Zero = FromDateTime(Epoch);

    private readonly TimeSpan _timeSpan;
    public long Ticks => _timeSpan.Ticks;

    public UnixTimeStamp(long ticks)
    {
        _timeSpan = new TimeSpan(ticks);
    }

    public UnixTimeStamp(TimeSpan timeSpan)
    {
        _timeSpan = timeSpan;
    }
    public long ToLong(UnixFormat format = UnixFormat.Seconds)
    {
        return format switch
        {
            UnixFormat.Seconds => _timeSpan.Seconds,
            UnixFormat.Milliseconds => _timeSpan.Milliseconds,
            UnixFormat.Microseconds => _timeSpan.Milliseconds * 1000,
            UnixFormat.Nanoseconds => _timeSpan.Ticks,
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
        };
    }

    /// <summary/>
    /// <returns>UTC DateTime</returns>
    public DateTime ToDateTime()
    {
        return Epoch + _timeSpan;
    }

    /// <summary>
    /// Converts DateTime to UTC format
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns>UTC UnixTimeStamp</returns>
    public static UnixTimeStamp FromDateTime(DateTime dateTime)
    {
        var timeSpan = dateTime.ToUniversalTime() - Epoch;
        return new UnixTimeStamp(timeSpan.Ticks);
    }

    public override bool Equals(object? obj)
    {
        return obj is UnixTimeStamp other && other._timeSpan.Equals(_timeSpan);
    }

    public override int GetHashCode()
    {
        return _timeSpan.GetHashCode();
    }

    public static bool operator ==(UnixTimeStamp left, UnixTimeStamp right)
    {
        return left.Equals(right);
    }

    public static bool operator !=(UnixTimeStamp left, UnixTimeStamp right)
    {
        return !(left == right);
    }
}