using JetBrains.Annotations;

namespace AchiesUtilities.Models;

[PublicAPI]
public class SetOnce<T>(T value)
{
    public T Value
    {
        get => _value;
        set => Set(value);
    }

    public bool IsSet { get; private set; }
    private T _value = value;

    /// <summary>
    /// </summary>
    /// <param name="value"></param>
    /// <exception cref="InvalidOperationException"></exception>
    public void Set(T value)
    {
        if (!TrySet(value))
            throw new InvalidOperationException("Value already set");
    }

    public bool TrySet(T value)
    {
        if (IsSet) return false;
        _value = value;
        IsSet = true;
        return true;
    }

    public T Get()
    {
        return Value;
    }

    /// <summary>
    ///     Set <see cref="IsSet" /> to <see langword="true" /> even if it contains default value
    /// </summary>
    public void Seal()
    {
        IsSet = true;
    }
}