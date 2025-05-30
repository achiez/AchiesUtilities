namespace AchiesUtilities.Collections;

internal class KeyIndex
{
    public int Index { get; private set; }
    public KeyIndex? NextIndexedValue { get; set; }
    public KeyIndex? PreviousIndexedValue { get; init; }

    public KeyIndex(int index)
    {
        Index = index;
    }

    public void RemoveIndex()
    {
        if (PreviousIndexedValue != null)
            PreviousIndexedValue.NextIndexedValue = NextIndexedValue;

        var next = NextIndexedValue;
        while (next != null)
        {
            next.Index = --next.Index;
            next = next.NextIndexedValue;
        }
    }
}