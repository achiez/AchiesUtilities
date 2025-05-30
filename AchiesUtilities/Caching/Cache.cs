using System.Collections.Concurrent;
using System.Diagnostics.CodeAnalysis;
using JetBrains.Annotations;

namespace AchiesUtilities.Caching;

[PublicAPI]
public sealed class Cache<TKey, TValue>
    where TKey : notnull
    where TValue : notnull
{
    internal CacheOptions<TValue> Options { get; }

    internal IEqualityComparer<TValue> ValueComparer { get; }
    private readonly ICacheSaver<TKey, TValue>? _cacheSaver;

    private readonly ConcurrentDictionary<TKey, CachedItem<TValue>> _entries;

    private DateTime _lastCheck = DateTime.MinValue;

    public Cache(CacheOptions<TValue> options, ICacheSaver<TKey, TValue>? saver = null)
    {
        var loaded = saver?.Load();
        _entries = new ConcurrentDictionary<TKey, CachedItem<TValue>>(loaded ??
                                                                      new Dictionary<TKey, CachedItem<TValue>>());
        if (loaded != null)
            this.FillComparerIn(_entries);

        _cacheSaver = saver;
        Options = options;
        ValueComparer = Options.ValueComparer ?? EqualityComparer<TValue>.Default;
    }

    public bool TryGetValue(TKey key, [NotNullWhen(true)] out TValue? value)
    {
        value = default;
        if (_entries.TryGetValue(key, out var entry))
        {
            if (IsExpired(key, entry))
                return false;

            value = entry.Value;
            ExecuteRegularOperations();
            return true;
        }

        ExecuteRegularOperations();
        return false;
    }


    public CacheResult TryCacheValue(TKey key, TValue value, out TValue? oldValue)
    {
        oldValue = default;
        var entry = this.CreateEntry(value);

        if (_entries.TryGetValue(key, out var oldEntry) == false || IsExpired(key, oldEntry))
        {
            ExecuteRegularOperations();
            return _entries.TryAdd(key, entry) ? CacheResult.Added : CacheResult.NoOp;
        }


        if (entry.Equals(oldEntry))
        {
            ExecuteRegularOperations();
            return CacheResult.Existed;
        }

        if (_entries.TryUpdate(key, entry, oldEntry))
        {
            oldValue = oldEntry.Value;
            ExecuteRegularOperations();
            return CacheResult.Updated;
        }

        ExecuteRegularOperations();
        return CacheResult.NoOp;
    }

    public Dictionary<TKey, TValue> GetCopy()
    {
        var arr = _entries.ToArray();
        return arr
            .Where(kvp => kvp.Value.IsExpired == false)
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value.Value);
    }

    public void SetExpired(TKey key)
    {
        if (_entries.TryGetValue(key, out var value))
        {
            value.SetExpired();
        }
    }

    public bool SetExpired(TKey key, TValue value)
    {
        if (!_entries.TryGetValue(key, out var cachedItem)) return true;
        if (!ValueComparer.Equals(value, cachedItem.Value)) return false;
        cachedItem.SetExpired();
        ExecuteRegularOperations();
        return true;
    }

    private bool IsExpired(TKey key, CachedItem<TValue> value)
    {
        if (value.IsExpired)
        {
            _entries.TryRemove(new KeyValuePair<TKey, CachedItem<TValue>>(key, value));
            return true;
        }

        return false;
    }

    private void ExecuteRegularOperations()
    {
        ScanExpirationIfNeeded();
        _cacheSaver?.SaveIfNeeded(_entries);
    }


    private void ScanExpirationIfNeeded()
    {
        if (Options.CheckEvery == Cache.NotSet) return;
        var now = DateTime.Now;
        if (_lastCheck + Options.CheckEvery < now)
        {
            ScheduleTask(now);
        }

        return;

        void ScheduleTask(DateTime time)
        {
            _lastCheck = time;
            Task.Factory.StartNew(state => ((Cache<TKey, TValue>) state!).ScanExpiration(), this,
                CancellationToken.None, TaskCreationOptions.DenyChildAttach, TaskScheduler.Default);
        }
    }

    private void ScanExpiration()
    {
        foreach (var kvp in _entries)
        {
            IsExpired(kvp.Key, kvp.Value);
        }
    }
}