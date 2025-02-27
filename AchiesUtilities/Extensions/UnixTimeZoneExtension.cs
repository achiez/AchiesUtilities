using AchiesUtilities.Models;
using JetBrains.Annotations;

namespace AchiesUtilities.Extensions;

/// <summary>
/// Class with extensions which helps when some service provides TimeStamp with offset according to its timezone
/// </summary>
[PublicAPI]
public static class UnixTimeZoneExtension
{
    public static long ToLongLocal(this  UnixTimeStamp unix, UnixFormat format = UnixFormat.Seconds)
        => ToLongWithTimeZone(unix, TimeZoneInfo.Local, format);

    public static long ToLongWithTimeZone(this UnixTimeStamp unix, TimeZoneInfo info, UnixFormat format = UnixFormat.Seconds)
    {
        var timespan = unix.TimeSpan + info.BaseUtcOffset;
        return format switch
        {
            UnixFormat.Seconds => timespan.Seconds,
            UnixFormat.Milliseconds => timespan.Milliseconds,
            UnixFormat.Microseconds => timespan.Microseconds,
            UnixFormat.Ticks => timespan.Ticks,
            _ => throw new ArgumentOutOfRangeException(nameof(format), format, null)
        };
    }
}