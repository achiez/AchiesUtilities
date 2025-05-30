using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;

namespace AchiesUtilities.Collections;

public class ReadOnlyObservableDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>, INotifyCollectionChanged,
    INotifyPropertyChanged where TKey : notnull
{
    public TValue this[TKey key] => _dictionary[key];

    public IEnumerable<TKey> Keys => _dictionary.Keys;
    public IEnumerable<TValue> Values => _dictionary.Values;
    public int Count => _dictionary.Count;
    private readonly ObservableDictionary<TKey, TValue> _dictionary;

    public ReadOnlyObservableDictionary(ObservableDictionary<TKey, TValue> dictionary)
    {
        _dictionary = dictionary;
        ((INotifyCollectionChanged) _dictionary).CollectionChanged +=
            HandleCollectionChanged;
        ((INotifyPropertyChanged) _dictionary).PropertyChanged +=
            HandlePropertyChanged;
    }

    event NotifyCollectionChangedEventHandler? INotifyCollectionChanged.CollectionChanged
    {
        add => CollectionChanged += value;
        remove => CollectionChanged -= value;
    }

    protected virtual event NotifyCollectionChangedEventHandler? CollectionChanged;

    protected virtual void OnCollectionChanged(NotifyCollectionChangedEventArgs args)
    {
        CollectionChanged?.Invoke(this, args);
    }

    event PropertyChangedEventHandler? INotifyPropertyChanged.PropertyChanged
    {
        add => PropertyChanged += value;
        remove => PropertyChanged -= value;
    }

    protected virtual event PropertyChangedEventHandler? PropertyChanged;

    /// <summary>
    ///     raise PropertyChanged event to any listeners
    /// </summary>
    protected virtual void OnPropertyChanged(PropertyChangedEventArgs args)
    {
        PropertyChanged?.Invoke(this, args);
    }

    private void HandleCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        OnCollectionChanged(e);
    }

    private void HandlePropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        OnPropertyChanged(e);
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }


    public bool ContainsKey(TKey key)
    {
        return _dictionary.ContainsKey(key);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return _dictionary.TryGetValue(key, out value);
    }
}