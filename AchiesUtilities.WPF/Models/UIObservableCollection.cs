using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace AchiesUtilities.WPF.Models;

public class UiObservableCollection<T> : ICollection<T>, INotifyCollectionChanged, INotifyPropertyChanged
{
    private readonly ObservableCollection<T> _collection;

    public UiObservableCollection(ObservableCollection<T> observableCollection)
    {
        _collection = observableCollection;
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

    public IEnumerator<T> GetEnumerator()
    {
        return _collection.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Add(T item)
    {
        _collection.Add(item);
    }

    public void Clear()
    {
        _collection.Clear();

    }

    public bool Contains(T item)
    {
        return _collection.Contains(item);
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
        _collection.CopyTo(array, arrayIndex);
    }

    public bool Remove(T item)
    {
        return _collection.Remove(item);
    }

    public int Count => _collection.Count;
    public bool IsReadOnly => false;

    #endregion

    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public event PropertyChangedEventHandler? PropertyChanged;


}