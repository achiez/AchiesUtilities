namespace AchiesUtilities.Caching;

public static class Cache
{
    public static readonly TimeSpan NotSet = new(0, 0, 0, 0, -1);

    internal static CachedItem<TValue> CreateEntry<TKey, TValue>(this Cache<TKey, TValue> cache, TValue value)
        where TValue : notnull where TKey : notnull
    {
        var comparer = cache.Options.ValueComparer ?? EqualityComparer<TValue>.Default;
        var expiration = cache.GetExpiration();
        return new CachedItem<TValue>(value, comparer, expiration);
    }

    internal static void FillComparerIn<TKey, TValue>(this Cache<TKey, TValue> cache,
        IDictionary<TKey, CachedItem<TValue>> entries) where TValue : notnull where TKey : notnull
    {
        foreach (var (key, cachedItem) in entries)
        {
            cachedItem.Comparer = cache.ValueComparer;
        }
    }

    internal static DateTime? GetExpiration<TKey, TValue>(this Cache<TKey, TValue> cache)
        where TKey : notnull where TValue : notnull
    {
        return cache.Options.EntryLifeTime == NotSet ? null : DateTime.Now + cache.Options.EntryLifeTime;
    }
}