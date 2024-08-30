using System.Reflection;
using JetBrains.Annotations;

namespace AchiesUtilities.Models;

[PublicAPI]
public class Singleton<T> where T : class
{
    private static readonly object _lock = new();
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
        lock (_lock)
        {
            if (_instance != null) return _instance;
            var ctor = typeof(T).GetConstructor(BindingFlags.NonPublic | BindingFlags.Instance, Type.EmptyTypes);
            if (ctor == null)
                throw new MissingMemberException($"Singleton {typeof(T)} must have private constructor without parameters");
            _instance = (T)ctor.Invoke(null);
            return _instance;
        }
    }
}