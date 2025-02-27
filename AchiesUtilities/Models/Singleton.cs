using System.Reflection;
using JetBrains.Annotations;

namespace AchiesUtilities.Models;

[PublicAPI]
public class Singleton<T> where T : class
{
    // ReSharper disable once StaticMemberInGenericType
    private static readonly Lock Lock = new();
   
    private static T? _instance;
    public static T Current
    {
        get
        {
            return _instance ??= Create();
        }
    }

    private static T Create()
    {
        lock (Lock)
        {
            if (_instance != null) return _instance;

            var ctor = typeof(T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, Type.EmptyTypes)
                       ?? throw new MissingMemberException($"Singleton {typeof(T)} must have private constructor without parameters");
           
            _instance = (T)ctor.Invoke(null);
            return _instance;
        }
    }
}