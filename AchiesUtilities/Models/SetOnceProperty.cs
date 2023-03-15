using JetBrains.Annotations;

namespace AchiesUtilities.Models;

[PublicAPI]
public class SetOnceProperty<T>
{
    private T _value;
    public T Value
    {
        get => _value;
        set => Set(value);
    }

    public bool IsSet { get; private set; }
    public SetOnceProperty(T defaultValue)
    {
        _value = defaultValue;
    }

    public void Set(T value)
    {
        if (IsSet)
            throw new InvalidOperationException("Value already set");
        IsSet = true;
        _value = value;
    }

    public bool TrySet(T value)
    {
        if (IsSet) return false;
        _value = value;
        return true;
    }

}