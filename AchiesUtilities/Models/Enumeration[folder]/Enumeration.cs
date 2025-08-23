using System.Collections.Immutable;
using JetBrains.Annotations;

namespace AchiesUtilities.Models;

[PublicAPI]
public abstract class Enumeration<TEnumeration> : IComparable, IEquatable<Enumeration<TEnumeration>>
    where TEnumeration : Enumeration<TEnumeration>
{
    public string Name { get; }
    public int Id { get; }

    private static EnumerationCache<TEnumeration> Cache { get; } = EnumerationCacheFactory.Create<TEnumeration>();

    private static ImmutableDictionary<Type, EnumerationCache<TEnumeration>> Caches { get; set; } =
        ImmutableDictionary<Type, EnumerationCache<TEnumeration>>.Empty;

    protected Enumeration(int id, string name)
    {
        Id = id;
        Name = name;
    }


    public TEnum AsEnum<TEnum>() where TEnum : Enum
    {
        return (TEnum) Enum.ToObject(typeof(TEnum), Id);
    }

    /// <summary>
    ///     Returns all values defined for <typeparamref name="TEnumeration" />.
    ///     This includes only the values declared in the type itself,
    ///     without taking derived types into account.
    /// </summary>
    public static IReadOnlyList<TEnumeration> GetAll()
    {
        return Cache.List;
    }

    /// <summary>
    ///     Returns all values for the specified type <typeparamref name="T" />.
    ///     <para>
    ///         The result includes every value declared in <typeparamref name="T" /> and in all of its base types,
    ///         up to and including <typeparamref name="TEnumeration" />.
    ///         In other words, the derived type <b>extends</b> the set of categories defined in the hierarchy;
    ///         none of the values are replaced or hidden.
    ///     </para>
    /// </summary>
    /// <typeparam name="T">
    ///     A type derived from <typeparamref name="TEnumeration" />
    ///     for which all available values across the inheritance chain should be retrieved.
    /// </typeparam>
    /// <returns>
    ///     A read-only collection containing all values declared in the entire type hierarchy
    ///     from <typeparamref name="T" /> down to <typeparamref name="TEnumeration" />.
    /// </returns>
    public static IReadOnlyList<TEnumeration> GetAll<T>() where T : TEnumeration
    {
        return GetCacheFor<T>().List;
    }

    private static EnumerationCache<TEnumeration> GetCacheFor<T>() where T : TEnumeration
    {
        if (!Caches.TryGetValue(typeof(T), out var cache))
        {
            cache = EnumerationCacheFactory.CreateFor<T, TEnumeration>();
            Caches = Caches.Add(typeof(T), cache);
        }

        return cache;
    }

    public static TEnumeration? FromId(int id)
    {
        return Cache.Dictionary.GetValueOrDefault(id);
    }

    public static TEnumeration? FromName(string name)
    {
        return Cache.NameDictionary.GetValueOrDefault(name);
    }

    public static TEnumeration? FromId<TEnum>(int id) where TEnum : TEnumeration
    {
        return GetCacheFor<TEnum>().Dictionary.GetValueOrDefault(id);
    }

    public static TEnumeration? FromName<TEnum>(string name) where TEnum : TEnumeration
    {
        return GetCacheFor<TEnum>().NameDictionary.GetValueOrDefault(name);
    }


    public override bool Equals(object? obj)
    {
        return obj is Enumeration<TEnumeration> other && Equals(other);
    }

    public bool Equals(Enumeration<TEnumeration>? other)
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
        return obj is Enumeration<TEnumeration> other ? Id.CompareTo(other.Id) : 1;
    }

    public static bool operator ==(Enumeration<TEnumeration>? left, Enumeration<TEnumeration>? right)
    {
        if (left is null) return right is null;
        return left.Equals(right);
    }

    public static bool operator !=(Enumeration<TEnumeration>? left, Enumeration<TEnumeration>? right)
    {
        return !(left == right);
    }

    public static bool operator <(Enumeration<TEnumeration>? left, Enumeration<TEnumeration>? right)
    {
        if (left is null || right is null) return false;
        return left.Id < right.Id;
    }

    public static bool operator >(Enumeration<TEnumeration>? left, Enumeration<TEnumeration>? right)
    {
        if (left is null || right is null) return false;
        return left.Id > right.Id;
    }

    public static bool operator <=(Enumeration<TEnumeration>? left, Enumeration<TEnumeration>? right)
    {
        if (left is null) return true;
        if (right is null) return false;
        return left.Id <= right.Id;
    }

    public static bool operator >=(Enumeration<TEnumeration>? left, Enumeration<TEnumeration>? right)
    {
        if (right is null) return true;
        if (left is null) return false;
        return left.Id >= right.Id;
    }
}