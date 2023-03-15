namespace AchiesUtilities.Caching;

public interface ICacheSaver<TKey, TValue> where TKey : notnull where TValue : notnull
{
    public TimeSpan SaveEvery { get; }
    public DateTime LastSaved { get; }
    public IDictionary<TKey, CachedItem<TValue>>? Load();

    public void SaveIfNeeded(IDictionary<TKey, CachedItem<TValue>> entries);
}