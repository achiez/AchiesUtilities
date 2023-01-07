using JetBrains.Annotations;

namespace AchiesUtilities;

[PublicAPI]
public class CachedItem<T>
{
    public T Value { get; }
    public DateTime CachedAt { get; set; }
    public DateTime ExpiresAt { get; }
    public bool Expired
    {
        get => _expired || DateTime.Now > ExpiresAt;
        set => _expired = value;
    }
    private bool _expired = false;

    public CachedItem(T item) : this(item, expiresAt: DateTime.MaxValue)
    {
        Value = item;
        ExpiresAt = DateTime.MaxValue;
    }

    public CachedItem(T item, DateTime expiresAt, DateTime? cachedAt = null)
    {
        Value = item;
        ExpiresAt = expiresAt;
        CachedAt = cachedAt ?? DateTime.Now;
    }
}