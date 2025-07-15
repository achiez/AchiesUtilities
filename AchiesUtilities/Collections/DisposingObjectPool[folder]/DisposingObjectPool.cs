using JetBrains.Annotations;
using System.Collections.Concurrent;

namespace AchiesUtilities.Collections;

[PublicAPI]
public class DisposingObjectPool<T> : IDisposable
    where T : IDisposable
{
    private readonly ConcurrentDictionary<string, DisposingPooledItem<T>> _items = new();
    public event EventHandler<PooledObjectDisposedEventArgs<T>>? ItemExpired;


    /// <summary>
    ///     Creates or gets existing <see cref="T" /> from pool. Every call updates lifetime of <see cref="T" /> to specified
    ///     <paramref name="lifeTime" />.
    /// </summary>
    /// <param name="itemName"></param>
    /// <param name="factory"></param>
    /// <param name="lifeTime"></param>
    /// <returns></returns>
    public T Get(string itemName, Func<T> factory, TimeSpan lifeTime)
    {
        if (lifeTime == TimeSpan.Zero)
            ArgumentOutOfRangeException.ThrowIfZero(lifeTime.Ticks, nameof(lifeTime));

        var pooled = _items.GetOrAdd(itemName, cn => Factory(cn, factory, lifeTime));
        pooled.SetLifeTime(lifeTime);
        var obj = pooled.GetItem();
        return pooled.Disposed ? Get(itemName, factory, lifeTime) : obj;
    }

    public void DisposeItem(string itemName)
    {
        if (!_items.TryGetValue(itemName, out var pooled)) return;
        pooled.ManualDispose();
    }

    private DisposingPooledItem<T> Factory(string key, Func<T> innerFactory, TimeSpan lifeTime)
    {
        var client = new DisposingPooledItem<T>(innerFactory, lifeTime,
            (item, manual) => ObjectDisposingCallback(item, manual, key));
        return client;
    }

    private void ObjectDisposingCallback(DisposingPooledItem<T> item, bool isManual, string key)
    {
        if (_items.TryRemove(KeyValuePair.Create(key, item)))
            ItemExpired?.Invoke(this, new PooledObjectDisposedEventArgs<T>(key, item.GetItem(), isManual));
    }


    public void Dispose()
    {
        foreach (var item in _items)
        {
            item.Value.ManualDispose();
        }

        _items.Clear();
        ItemExpired = null;
    }
}