using JetBrains.Annotations;

namespace AchiesUtilities.Models;

[PublicAPI]
public struct ExpirationWatch
{
    public DateTime Start { get; }
    public DateTime End { get; }
    public ExpirationWatch(TimeSpan expiration)
    {
        Start = DateTime.Now;
        End = Start + expiration;
    }

    public ExpirationWatch(DateTime start, TimeSpan expiration)
    {
        Start = start;
        End = start + expiration;
    }


    public bool IsExpired()
    {
        return End < DateTime.Now;
    }
}