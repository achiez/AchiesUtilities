namespace AchiesUtilities.Collections;

internal sealed class DisposingPooledItem<T> where T : IDisposable
{
    public bool Disposed => _disposed == 1;


    private Action<DisposingPooledItem<T>, bool> _callback;
    private int _disposed;
    private T _item;
    private TimeSpan _lifetime;
    private Timer _timer;

    public DisposingPooledItem(Func<T> objectFactory, TimeSpan lifeTime, Action<DisposingPooledItem<T>, bool> callback)
    {
        _item = objectFactory.Invoke();
        _timer = new Timer(DisposeItem, null, Timeout.InfiniteTimeSpan, Timeout.InfiniteTimeSpan);
        SetLifeTime(lifeTime);
        _callback = callback;
    }

    public T GetItem()
    {
        return _item;
    }

    public void ManualDispose()
    {
        DisposeItem(true);
    }

    public void SetLifeTime(TimeSpan value)
    {
        if (Disposed) return;
        if (value == Timeout.InfiniteTimeSpan && _lifetime == value)
            return;

        _lifetime = value;
        _timer.Change(_lifetime, Timeout.InfiniteTimeSpan);
    }

    private void DisposeItem(object? state)
    {
        if (Interlocked.CompareExchange(ref _disposed, 1, 0) != 0) return;
        _timer.Dispose();
        _callback.Invoke(this, state is true);
        _item.Dispose();

        _item = default!;
        _timer = null!;
        _callback = null!;
    }
}