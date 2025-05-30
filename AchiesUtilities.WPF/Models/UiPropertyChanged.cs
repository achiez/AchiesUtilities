using System.ComponentModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;

namespace AchiesUtilities.WPF.Models;

[INotifyPropertyChanged]
public abstract partial class UiPropertyChanged<T> where T : INotifyPropertyChanged
{
    public readonly T Object;

    protected UiPropertyChanged(T obj)
    {
        Object = obj;
        obj.PropertyChanged += ObjOnPropertyChanged;
    }

    protected virtual void ObjOnPropertyChanged(object? sender, PropertyChangedEventArgs e)
    {
        Application.Current.Dispatcher.BeginInvoke(() => { PropertyChanged?.Invoke(this, e); });
    }
}