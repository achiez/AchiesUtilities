using System.Reflection;

namespace AchiesUtilities.Models;

internal static class EnumerationCacheFactory
{
    public static EnumerationCache<TEnum> Create<TEnum>() where TEnum : Enumeration<TEnum>
    {
        var fields = typeof(TEnum)
            .GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(TEnum))
            .Select(f => (TEnum) f.GetValue(null)!)
            .ToList();


        return new EnumerationCache<TEnum>(fields.AsReadOnly());
    }

    public static EnumerationCache<TBase> CreateFor<TEnum, TBase>()
        where TBase : Enumeration<TBase>
        where TEnum : TBase
    {
        var fields = typeof(TEnum)
            .GetInheritanceChainUntil(typeof(TBase))
            .SelectMany(t => t.GetFields(BindingFlags.Public | BindingFlags.Static))
            .Where(f => f.FieldType.IsAssignableTo(typeof(TBase)))
            .Select(f => (TBase) f.GetValue(null)!)
            .ToList();

        return new EnumerationCache<TBase>(fields.AsReadOnly());
    }

    private static IEnumerable<Type> GetInheritanceChainUntil(this Type type, Type stopType)
    {
        for (var t = type; t != null && t != stopType.BaseType; t = t.BaseType)
            yield return t;
    }
}

internal class EnumerationCache<TEnum> where TEnum : Enumeration<TEnum>
{
    public Dictionary<int, TEnum> Dictionary { get; }
    public Dictionary<string, TEnum> NameDictionary { get; }
    public IReadOnlyList<TEnum> List { get; }

    public EnumerationCache(IReadOnlyList<TEnum> fields)
    {
        List = fields;
        Dictionary = fields.ToDictionary(e => e.Id);
        NameDictionary = fields.ToDictionary(e => e.Name);
    }
}