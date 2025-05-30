namespace AchiesUtilities.Collections;

public class PooledObjectDisposedEventArgs<T>(string key, T item, bool isManual = false)
    : EventArgs where T : IDisposable
{
    public string Key { get; } = key;
    public T Item { get; } = item;
    public bool IsManual { get; } = isManual;
}