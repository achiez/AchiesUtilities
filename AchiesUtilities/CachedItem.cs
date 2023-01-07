using System.Text.Json.Serialization;
using JetBrains.Annotations;

namespace AchiesUtilities;

[PublicAPI]
[Serializable]
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

    public CachedItem(T value) : this(value, expiresAt: DateTime.MaxValue)
    { }

    [Newtonsoft.Json.JsonConstructor]
    [JsonConstructor]
    public CachedItem(T value, DateTime expiresAt, DateTime? cachedAt = null)
    {
        Value = value;
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