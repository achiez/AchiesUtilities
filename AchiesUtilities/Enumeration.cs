using System.Reflection;
using JetBrains.Annotations;

namespace AchiesUtilities;

[PublicAPI]
public abstract class Enumeration : IComparable
{
    public string Name { get; }
    public int Id { get; }

    protected Enumeration(int id, string name) => (Id, Name) = (id, name);

    public static IEnumerable<T> GetAll<T>() where T : Enumeration
    {
        return 
            typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<T>();
    }

    public static IEnumerable<Enumeration> GetAll(Type type)
    {
        if (type.IsSubclassOf(typeof(Enumeration)) == false)
            throw new InvalidOperationException("GetAll method can be used only on Enumeration");

        return 
            type.GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly)
            .Select(f => f.GetValue(null))
            .Cast<Enumeration>();
    }

    public override bool Equals(object? obj)
    {
        if (obj is not Enumeration otherValue)
        {
            return false;
        }
        return GetType() == obj.GetType() && Id.Equals(otherValue.Id);
    }
    public override string ToString() => Name;
    public override int GetHashCode()
    {
        return HashCode.Combine(Name, Id, GetType());
    }

    public virtual int CompareTo(object? other)
    {
        if (other is Enumeration e)
        {
            return Id.CompareTo(e.Id);
        }
        else
        {
            return 1;
        }
    }
}