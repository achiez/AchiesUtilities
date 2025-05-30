using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Windows;

namespace AchiesUtilities.WPF.Models;

public class RoUiObservableCollection<T> : IReadOnlyCollection<T>, INotifyCollectionChanged, INotifyPropertyChanged
{
    private readonly ReadOnlyObservableCollection<T> _collection;

    public RoUiObservableCollection(ReadOnlyObservableCollection<T> readOnlyObservableCollection)
    {
        _collection = readOnlyObservableCollection;
        ((INotifyCollectionChanged) _collection).CollectionChanged += OnCollectionChanged;
        ((INotifyPropertyChanged) _collection).PropertyChanged += OnPropertyChanged;
    }

    private void OnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        Application.Current.Dispatcher.BeginInvoke(() => { PropertyChanged?.Invoke(this, e); });
    }

    private void OnCollectionChanged(object? sender, NotifyCollectionChangedEventArgs e)
    {
        Application.Current.Dispatcher.BeginInvoke(() => { CollectionChanged?.Invoke(this, e); });
    }


    public event NotifyCollectionChangedEventHandler? CollectionChanged;
    public event PropertyChangedEventHandler? PropertyChanged;

    #region Wrapper

    public IEnumerator<T> GetEnumerator()
    {
        return _collection.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public int Count => _collection.Count;

    #endregion
}