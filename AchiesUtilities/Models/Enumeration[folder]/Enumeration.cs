using JetBrains.Annotations;

namespace AchiesUtilities.Models;

[PublicAPI]
public abstract class Enumeration<TEnum> : IComparable, IEquatable<Enumeration<TEnum>>
    where TEnum : Enumeration<TEnum>
{
    public string Name { get; }
    public int Id { get; }

    private static EnumerationCache<TEnum> Cache { get; } = EnumerationCacheFactory.Create<TEnum>();

    protected Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }

    public static IReadOnlyList<TEnum> GetAll()
    {
        return Cache.List;
    }

    public static TEnum? FromId(int id)
    {
        return Cache.Dictionary.GetValueOrDefault(id);
    }

    public static TEnum? FromName(string name)
    {
        return Cache.NameDictionary.GetValueOrDefault(name);
    }

    public override bool Equals(object? obj)
    {
        return obj is Enumeration<TEnum> other && Equals(other);
    }

    public bool Equals(Enumeration<TEnum>? other)
    {
        return other is not null && Id == other.Id;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Id);
    }

    public override string ToString()
    {
        return Name;
    }

    public int CompareTo(object? obj)
    {
        return obj is Enumeration<TEnum> other ? Id.CompareTo(other.Id) : 1;
    }

    public static bool operator ==(Enumeration<TEnum>? left, Enumeration<TEnum>? right)
    {
        if (left is null) return right is null;
        return left.Equals(right);
    }

    public static bool operator !=(Enumeration<TEnum>? left, Enumeration<TEnum>? right)
    {
        return !(left == right);
    }

    public static bool operator <(Enumeration<TEnum>? left, Enumeration<TEnum>? right)
    {
        if (left is null || right is null) return false;
        return left.Id < right.Id;
    }

    public static bool operator >(Enumeration<TEnum>? left, Enumeration<TEnum>? right)
    {
        if (left is null || right is null) return false;
        return left.Id > right.Id;
    }

    public static bool operator <=(Enumeration<TEnum>? left, Enumeration<TEnum>? right)
    {
        if (left is null) return true;
        if (right is null) return false;
        return left.Id <= right.Id;
    }

    public static bool operator >=(Enumeration<TEnum>? left, Enumeration<TEnum>? right)
    {
        if (right is null) return true;
        if (left is null) return false;
        return left.Id >= right.Id;
    }
}