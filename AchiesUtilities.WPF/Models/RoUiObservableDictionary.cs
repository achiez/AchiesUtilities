using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;
using AchiesUtilities.Collections;

namespace AchiesUtilities.WPF.Models;

public class RoUiObservableDictionary<TKey, TValue> : 
    IReadOnlyDictionary<TKey, TValue>, INotifyCollectionChanged,
    INotifyPropertyChanged where TKey : notnull
{
    private readonly ReadOnlyObservableDictionary<TKey, TValue> _collection;
    public RoUiObservableDictionary(ReadOnlyObservableDictionary<TKey, TValue> dictionary)
    {
        _collection = dictionary;
        ((INotifyCollectionChanged)_collection).CollectionChanged += OnCollectionChanged;
        ((INotifyPropertyChanged)_collection).PropertyChanged += OnPropertyChanged;
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        Application.Current.Dispatcher.BeginInvoke(() =>
        {
            PropertyChanged?.Invoke(this, e);
        });
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        Application.Current.Dispatcher.BeginInvoke(() =>
        {
            CollectionChanged?.Invoke(this, e);
        });
    }

    #region Wrapper

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return _collection.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count => _collection.Count;
    public bool ContainsKey(TKey key)
    {
        return _collection.ContainsKey(key);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return _collection.TryGetValue(key, out value);
    }

    public TValue this[TKey key] => _collection[key];

    public IEnumerable<TKey> Keys => _collection.Keys;
    public IEnumerable<TValue> Values => _collection.Values;

    #endregion

    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public event PropertyChangedEventHandler? PropertyChanged;
}