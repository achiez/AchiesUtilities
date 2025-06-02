using System.Collections;

namespace AchiesUtilities.Logging;

internal class MergedLogState<TState> : IReadOnlyList<KeyValuePair<string, object?>>
{
    private readonly TState _originalState;
    private readonly IReadOnlyList<KeyValuePair<string, object?>> _merged;

    public MergedLogState(TState originalState, IEnumerable<KeyValuePair<string, object?>> additional)
    {
        _originalState = originalState;

        if (originalState is not IReadOnlyList<KeyValuePair<string, object?>> originalList)
        {
            throw new InvalidOperationException("State must be a list of log values");
        }

        var merged = new List<KeyValuePair<string, object?>>(originalList);

        foreach (var pair in additional)
        {
            if (merged.All(p => p.Key != pair.Key))
                merged.Add(pair);
        }

        _merged = merged;
    }

    public KeyValuePair<string, object?> this[int index] => _merged[index];
    public int Count => _merged.Count;
    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator() => _merged.GetEnumerator();
    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public override string ToString() => _originalState?.ToString() ?? "";
}