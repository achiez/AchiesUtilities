using System.Collections;
using System.Collections.ObjectModel;
using JetBrains.Annotations;


namespace AchiesUtilities.Extensions;

[PublicAPI]
public static class IEnumerableExtension
{
    /// <summary>
    /// Slow method <b>O(n)</b>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <param name="other"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static bool CompareToOther<T>(this IEnumerable<T> collection, IEnumerable<T> other, EqualityComparer<T>? comparer = null)
    {
        comparer ??= EqualityComparer<T>.Default;
        var copyFirst = new List<T>(collection);
        var copySecond = new List<T>(other);
        if (copyFirst.Count != copySecond.Count)
        {
            return false;
        }

        for (var i = 0; i < copyFirst.Count;)
        {
            if (copyFirst.Count == 0) break;
            var first = copyFirst[i];
            var second = copySecond.FirstOrDefault(item => comparer.Equals(item));
            if (second == null || comparer.Equals(first, second) == false)
                return false;
            
            copyFirst.RemoveAt(i);
            var indexSecond = copySecond.IndexOf(second);
            copySecond.RemoveAt(indexSecond);
        }

        return true;
    }

    /// <summary>
    /// Fast method
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <param name="comparer"></param>
    /// <returns></returns>
    public static bool CompareToOther<T>(this HashSet<T> first, HashSet<T> second, EqualityComparer<T>? comparer = null)
    {
        if(first.Count != second.Count) return false;
        var copyFirst = new HashSet<T>(first, comparer);
        var copySecond = new HashSet<T>(second, comparer);

        return copyFirst.All(copySecond.Remove);
    }

    /// <summary>
    /// Average speed
    /// </summary>
    /// <typeparam name="TK"></typeparam>
    /// <typeparam name="TV"></typeparam>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <param name="keyComparer"></param>
    /// <param name="valueComparer"></param>
    /// <returns></returns>
    public static bool CompareToOther<TK, TV>(this IDictionary<TK,TV> first, IDictionary<TK,TV> second, 
        EqualityComparer<TK>? keyComparer = null, 
        EqualityComparer<TV>? valueComparer = null) where TK : notnull
    {
        if (first.Count != second.Count) return false;
        valueComparer ??= EqualityComparer<TV>.Default;
        var copyFirst = new Dictionary<TK, TV>(first, keyComparer);
        var copySecond = new Dictionary<TK, TV>(second, keyComparer);

        foreach (var kvp in copyFirst)
        {
            if (copySecond.TryGetValue(kvp.Key, out var value))
            {
                if (valueComparer.Equals(kvp.Value, value) == false) return false;
            }
            else
            {
                return false;
            }
        }

        return true;
    }
    /// <summary>
    /// Fast method
    /// </summary>
    /// <typeparam name="TK"></typeparam>
    /// <typeparam name="TV"></typeparam>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <param name="keyComparer"></param>
    /// <returns></returns>
    public static bool CompareToOtherByKey<TK, TV>(this IDictionary<TK, TV> first, IDictionary<TK, TV> second,
        EqualityComparer<TK>? keyComparer = null) where TK : notnull
    {
        if (first.Count != second.Count) return false;
        var copyFirst = new Dictionary<TK, TV>(first, keyComparer);
        var copySecond = new Dictionary<TK, TV>(second, keyComparer);

        foreach (var kvp in copyFirst)
        {
            if (copySecond.TryGetValue(kvp.Key, out _) == false)
            {
                return false;
            }
        }

        return true;
    }
}