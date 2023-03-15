using JetBrains.Annotations;
using Newtonsoft.Json;

namespace AchiesUtilities.Caching;

[PublicAPI]
public class CachedItem<TValue> : IEquatable<CachedItem<TValue>>
    where TValue : notnull
{
    public TValue Value { get; }
    public DateTime? ExpirationTime { get; private set; }

    [JsonIgnore]
    internal IEqualityComparer<TValue> Comparer { get; set; }

    internal bool IsExpired => ExpirationTime.HasValue && DateTime.Now > ExpirationTime.Value;
    public CachedItem(TValue value, IEqualityComparer<TValue> comparer, DateTime? expirationTime)
    {
        Comparer = comparer;
        Value = value;
        ExpirationTime = expirationTime;
    }

    public void SetExpired() => ExpirationTime = DateTime.MinValue;

    public override bool Equals(object? obj) => obj is CachedItem<TValue> i && Equals(i);
    public override int GetHashCode() => Value.GetHashCode();

    public bool Equals(CachedItem<TValue>? other)
    {
        if (other == null) return false;
        return ReferenceEquals(this, other) || Comparer.Equals(Value, other.Value);
    }

}