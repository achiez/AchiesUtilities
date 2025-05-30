using System.Reflection;

namespace AchiesUtilities.Models;

internal static class EnumerationCacheFactory
{
    public static EnumerationCache<TEnum> Create<TEnum>() where TEnum : Enumeration<TEnum>
    {
        var fields = typeof(TEnum).GetFields(BindingFlags.Public | BindingFlags.Static)
            .Where(f => f.FieldType == typeof(TEnum))
            .Select(f => (TEnum) f.GetValue(null)!)
            .ToList();


        return new EnumerationCache<TEnum>(fields.AsReadOnly());
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