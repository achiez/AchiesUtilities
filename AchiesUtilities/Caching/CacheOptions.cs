using JetBrains.Annotations;

namespace AchiesUtilities.Caching;

[PublicAPI]
public class CacheOptions<TValue>

where TValue : notnull
{
    public IEqualityComparer<TValue>? ValueComparer { get; init; }
    public TimeSpan CheckEvery { get; init; } = Cache.NotSet;
    public TimeSpan EntryLifeTime { get; set; } = Cache.NotSet;

    public CacheOptions(IEqualityComparer<TValue> comparer)
    {
        ValueComparer = comparer;
    }

    public CacheOptions() { }
}