using JetBrains.Annotations;

namespace AchiesUtilities;

[PublicAPI]
public struct CachedItem<T>
{
    public T Value { get; }
    public DateTime CachedAt { get; }
    public DateTime ExpiresAt { get; }
    public bool Expired
    {
        get => _expired || DateTime.Now > ExpiresAt;
        set => _expired = value;
    }
    private bool _expired = false;

    public CachedItem(T item) : this(item, expiresAt: DateTime.MaxValue)
    { }

    public CachedItem(T item, DateTime expiresAt, DateTime? cachedAt = null)
    {
        Value = item;
        ExpiresAt = expiresAt;
        CachedAt = cachedAt ?? DateTime.Now;
    }

    public override bool Equals(object? obj)
    {
        return obj is CachedItem<T> i && Equals(i);
    }

    public bool Equals(CachedItem<T> other)
    {
        return _expired == other._expired 
               && EqualityComparer<T>.Default.Equals(Value, other.Value) 
               && CachedAt.Equals(other.CachedAt) 
               && ExpiresAt.Equals(other.ExpiresAt);
    }
    public override int GetHashCode()
    {
        return HashCode.Combine(_expired, Value, CachedAt, ExpiresAt);
    }
}