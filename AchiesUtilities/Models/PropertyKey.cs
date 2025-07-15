namespace AchiesUtilities.Models;

public readonly struct PropertyKey : IEquatable<PropertyKey>
{
    public string Name { get; }

    public PropertyKey(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public static implicit operator string(PropertyKey key)
    {
        return key.Name;
    }

    public static implicit operator PropertyKey(string name)
    {
        return new PropertyKey(name);
    }

    public bool Equals(PropertyKey other)
    {
        return Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        return obj is PropertyKey other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}

public readonly struct PropertyKey<T> : IEquatable<PropertyKey<T>>
{
    public string Name { get; }

    public PropertyKey(string name)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
    }

    public static implicit operator string(PropertyKey<T> key)
    {
        return key.Name;
    }

    public static implicit operator PropertyKey<T>(string name)
    {
        return new PropertyKey<T>(name);
    }

    public bool Equals(PropertyKey<T> other)
    {
        return Name == other.Name;
    }

    public override bool Equals(object? obj)
    {
        return obj is PropertyKey<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return Name.GetHashCode();
    }
}