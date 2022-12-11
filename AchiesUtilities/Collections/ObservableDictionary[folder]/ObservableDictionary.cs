using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace AchiesUtilities.Collections;

public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>,
    IDictionary,
    ISerializable,
    IDeserializationCallback,
    INotifyCollectionChanged,
    INotifyPropertyChanged where TKey : notnull
{
    private readonly ISerializable _serializableImplementation;
    private readonly IDeserializationCallback _deserializationCallbackImplementation;
    private readonly IDictionary _dictionaryImplementation;
    private readonly Dictionary<TKey, TValue> _dictionary;
    private readonly Dictionary<TKey, KeyIndex> _indices = new();
    private KeyIndex? _last;
    public ObservableDictionary() : this(new Dictionary<TKey, TValue>())
    { }

    /// <summary>
    /// Slow
    /// </summary>
    /// <param name="dictionary"></param>
    public ObservableDictionary(Dictionary<TKey, TValue> dictionary)
    {
        _serializableImplementation = dictionary;
        _dictionaryImplementation = dictionary;
        _deserializationCallbackImplementation = dictionary;
        _dictionary = dictionary;
        var i = 0;
        foreach (var kvp in dictionary)
        {
            var keyIndex = new KeyIndex(i)
            {
                PreviousIndexedValue = _last
            };
            if (_last != null)
            {
                _last.NextIndexedValue = keyIndex;
            }

            _indices.Add(kvp.Key, keyIndex);
            _last = keyIndex;
            i++;
        }
    }

    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

  

    #region Implementation of Dictionary<,>

    #region NotRaisingEvent

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
    {
        return _dictionary.GetEnumerator();
    }


    public ICollection<TValue> Values => _dictionary.Values;

    public ICollection<TKey> Keys => _dictionary.Keys;

    IEnumerator IEnumerable.GetEnumerator()
    {
        return ((IEnumerable)_dictionaryImplementation).GetEnumerator();
    }

    public void CopyTo(Array array, int index)
    {
        _dictionaryImplementation.CopyTo(array, index);
    }

    public int Count => _dictionaryImplementation.Count;
    public bool IsReadOnly => false;

    public bool IsSynchronized => _dictionaryImplementation.IsSynchronized;

    public object SyncRoot => _dictionaryImplementation.SyncRoot;
    #endregion

    public void Add(TKey key, TValue value)
    {
        AddEntry(key, value);
    }

    public bool ContainsKey(TKey key)
    {
        return _dictionary.ContainsKey(key);
    }

    public bool Remove(TKey key)
    {
        return RemoveEntry(key);
    }

    public bool TryGetValue(TKey key, out TValue value)
    {
        return _dictionary.TryGetValue(key, out value);
    }

    public TValue this[TKey key]
    {
        get => _dictionary[key];
        set => SetEntry(key, value);
    }

    #endregion

    private void AddEntry(TKey key, TValue value)
    {
        _dictionary.Add(key, value);
        var index = _dictionary.Count - 1;
        var keyIndex = new KeyIndex(index)
        {
            PreviousIndexedValue = _last,
        };
        if (_last != null)
        {
            _last.NextIndexedValue = keyIndex;
        }
        _last = keyIndex;
        _indices.Add(key, keyIndex);
        FirePropertyChanged();
        FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add,  KvpShortcut(key, value), index));
    }

    private bool RemoveEntry(TKey key)
    {
        if (_dictionary.TryGetValue(key, out var value) == false) return false;
        var index = _indices[key];
        var removedIndex = index.Index;
        index.RemoveIndex();
        _dictionary.Remove(key);
        _indices.Remove(key);
        FirePropertyChanged();
        FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Remove, KvpShortcut(key, value), removedIndex));
        return true;
    }

    private void SetEntry(TKey key, TValue value)
    {
        if (_dictionary.TryGetValue(key, out var old))
        {
            _dictionary[key] = value;
            var index = _indices[key].Index;
            FirePropertyChanged();
            FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Replace, KvpShortcut(key, value), KvpShortcut(key, old), index));
        }
        else
        {
            AddEntry(key, value);
        }
    }
    private void ClearEntries()
    {
        _dictionary.Clear();
        _indices.Clear();
        FirePropertyChanged();
        FireCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
    private void FireCollectionChanged(NotifyCollectionChangedEventArgs args)
    {
        CollectionChanged?.Invoke(this, args);
    }
    private void FirePropertyChanged()
    {
        OnPropertyChanged("Count");
        OnPropertyChanged("Item[]");
        OnPropertyChanged("Keys");
        OnPropertyChanged("Values");
    }
    private void OnPropertyChanged(string property)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
    }
    private static KeyValuePair<TKey, TValue> KvpShortcut(TKey key, TValue value)
    {
        return new KeyValuePair<TKey, TValue>(key, value);
    }

    #region OtherImplementations

    void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
    {
        _serializableImplementation.GetObjectData(info, context);
    }

    void IDeserializationCallback.OnDeserialization(object? sender)
    {
        _deserializationCallbackImplementation.OnDeserialization(sender);
    }
    bool ICollection<KeyValuePair<TKey, TValue>>.Contains(KeyValuePair<TKey, TValue> item)
    {
        return _dictionary.Contains(item);
    }
    void ICollection<KeyValuePair<TKey, TValue>>.CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
    { 
        ((ICollection<KeyValuePair<TKey, TValue>>)_dictionary).CopyTo(array, arrayIndex);
    }

    bool IDictionary.Contains(object key)
    {
        return _dictionaryImplementation.Contains(key);
    }
    IDictionaryEnumerator IDictionary.GetEnumerator()
    {
        return _dictionaryImplementation.GetEnumerator();
    }
    bool IDictionary.IsFixedSize => _dictionaryImplementation.IsFixedSize;

     bool IDictionary.IsReadOnly => _dictionaryImplementation.IsReadOnly;

    ICollection IDictionary.Keys => _dictionaryImplementation.Keys;
    ICollection IDictionary.Values => _dictionaryImplementation.Values;

    #endregion

    #region OtherImplRaisingEvent

    void IDictionary.Add(object key, object? value)
    {
        AddEntry((TKey) key, TryCastValue(value));
    }

    void ICollection<KeyValuePair<TKey, TValue>>.Add(KeyValuePair<TKey, TValue> item)
    {
        ClearEntries();
    }

    public void Clear()
    {
        ClearEntries();
    }

    bool ICollection<KeyValuePair<TKey, TValue>>.Remove(KeyValuePair<TKey, TValue> item)
    {
        if (_dictionary.TryGetValue(item.Key, out var value))
        {
            if (EqualityComparer<TValue>.Default.Equals(value, item.Value))
            {
                return _dictionary.Remove(item.Key);
            }
        }
        return false;
    }
    void IDictionary.Remove(object key)
    {
        _dictionaryImplementation.Remove(key);
    }
    public object? this[object key]
    {
        get => _dictionaryImplementation[key];
        set => SetEntry((TKey)key, TryCastValue(value));
    }
    #endregion

    private TValue? TryCastValue(object? value)
    {
        if (value == null && Nullable.GetUnderlyingType(typeof(TValue)) != null)
        {
            throw new ArgumentNullException(nameof(value));
        }
        else if(value == null)
        {
            return (TValue?)value;
        }
        else if( (TValue)value == null)
        {
            throw new InvalidCastException($"Can't cast value of type {value!.GetType()} to type {typeof(TValue)}");
        }
        return (TValue)value;
        
    }
}