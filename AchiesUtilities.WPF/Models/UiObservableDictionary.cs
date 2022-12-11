using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Windows;
using AchiesUtilities.Collections;

namespace AchiesUtilities.WPF.Models;

public class UiObservableDictionary<TKey, TValue> :
    IDictionary<TKey, TValue>,
    IDictionary,
    ISerializable,
    IDeserializationCallback,
    INotifyCollectionChanged,
    INotifyPropertyChanged where TKey : notnull
{
    private readonly ObservableDictionary<TKey, TValue> _collection;

    public UiObservableDictionary(ObservableDictionary<TKey, TValue> dictionary)
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

    bool IDictionary.Contains(object key)
    {
        return ((IDictionary)_collection).Contains(key);
    }

    IDictionaryEnumerator IDictionary.GetEnumerator()
    {
        return ((IDictionary)_collection).GetEnumerator();
    }

    void IDictionary.Remove(object key)
    {
        ((IDictionary)_collection).Remove(key);
    }

    bool IDictionary.IsFixedSize => ((IDictionary) _collection).IsFixedSize;
    bool IDictionary.IsReadOnly => ((IDictionary) _collection).IsReadOnly;

    object? IDictionary.this[object key]
    {
        get => ((IDictionary) _collection)[key];
        set => ((IDictionary) _collection)[key] = value;
    }

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return _collection.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return _collection.GetEnumerator();
    }

    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
    {
        ((ICollection<KeyValuePair<TKey, TValue>>)_collection).Add(item);
    }

    void IDictionary.Add(object key, object? value)
    {
        ((IDictionary)_collection).Add(key, value);
    }

    void IDictionary.Clear()
    {
        _collection.Clear();
    }

    void ICollection<KeyValuePair<TKey, TValue>>.Clear()
    {
        _collection.Clear();;
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
    {
       return ((IDictionary)_collection).Contains(item);
    }

    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    {
        ((IDictionary)_collection).CopyTo(array, arrayIndex);
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
    {
         return ((ICollection<KeyValuePair<TKey, TValue>>)_collection).Remove(item);
    }

    void ICollection.CopyTo(Array array, int index)
    {
        ((ICollection)_collection).CopyTo(array, index);
    }

    int ICollection.Count => _collection.Count;

    bool ICollection.IsSynchronized => ((ICollection) _collection).IsSynchronized;
    object ICollection.SyncRoot => ((ICollection)_collection).SyncRoot;

    int ICollection<KeyValuePair<TKey, TValue>>.Count => _collection.Count;

    bool ICollection<KeyValuePair<TKey, TValue>>.IsReadOnly => ((IDictionary)_collection).IsReadOnly;

    public void Add(TKey key, TValue value)
    {
        _collection.Add(key, value);
    }

    public bool ContainsKey(TKey key)
    {
        return _collection.ContainsKey(key);
    }

    public bool Remove(TKey key)
    {
        return _collection.Remove(key);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return _collection.TryGetValue(key, out value);
    }

    public TValue this[TKey key]
    {
        get => _collection[key];
        set => _collection[key] = value;
    }

    ICollection<TKey> IDictionary<TKey, TValue>.Keys => _collection.Keys;

    ICollection IDictionary.Values => ((IDictionary)_collection).Values;

    ICollection IDictionary.Keys => ((IDictionary)_collection).Keys;

    ICollection<TValue> IDictionary<TKey, TValue>.Values => _collection.Values;

    public void GetObjectData(SerializationInfo info, StreamingContext context)
    {
        ((ISerializable) _collection).GetObjectData(info, context);
    }

    public void OnDeserialization(object? sender)
    {
        ((IDeserializationCallback) _collection).OnDeserialization(sender);
    }

    #endregion

    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public event PropertyChangedEventHandler? PropertyChanged;
}