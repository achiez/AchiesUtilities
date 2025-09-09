using JetBrains.Annotations;

namespace AchiesUtilities.Collections;

/// <summary>
///     A simple versioned ring buffer that stores the most recent items
///     and allows replaying them starting from a given version.
/// </summary>
/// <typeparam name="T">The type of items stored in the buffer.</typeparam>
/// <remarks>
///     <para>
///         This buffer is intended for basic scenarios where you only need:
///     </para>
///     <list type="bullet">
///         <item>Appending new items with an explicit version number.</item>
///         <item>Keeping only the last <c>capacity</c> items (older ones are overwritten).</item>
///         <item>Replaying items starting from a specific version.</item>
///     </list>
///     <para>
///         It does not handle advanced cases such as integer overflow or concurrent access.
///     </para>
/// </remarks>
[PublicAPI]
public class VersionedReplayBuffer<T>
{
    /// <summary>
    ///     The version of the oldest available item in the buffer.
    /// </summary>
    public long MinVersion => Count > 0 ? _minVersion : 0;

    /// <summary>
    ///     The version of the most recently added item.
    /// </summary>
    public long MaxVersion { get; private set; }

    /// <summary>
    ///     The number of items currently stored in the buffer.
    /// </summary>
    public int Count { get; private set; }

    private readonly T[] _buffer;
    private readonly long[] _versions;
    private int _head;
    private long _minVersion;

    public VersionedReplayBuffer(int capacity)
    {
        _buffer = new T[capacity];
        _versions = new long[capacity];
    }

    /// <summary>
    ///     Adds a new item with the given version to the buffer.
    ///     If the buffer is full, the oldest item is overwritten.
    /// </summary>
    /// <param name="version">The version number associated with the item.</param>
    /// <param name="item">The item to store.</param>
    public void Add(long version, T item)
    {
        _buffer[_head] = item;
        _versions[_head] = version;

        _head = (_head + 1) % _buffer.Length;
        if (Count < _buffer.Length)
        {
            Count++;
        }
        else
        {
            _minVersion = _versions[_head];
        }

        MaxVersion = version;
    }

    /// <summary>
    ///     Returns items starting from the given version up to the newest item.
    /// </summary>
    /// <param name="version">
    ///     The minimum version to start replaying from.
    ///     Must be between <see cref="MinVersion" /> and <see cref="MaxVersion" />.
    /// </param>
    /// <returns>All items with version greater than or equal to the requested version.</returns>
    /// <exception cref="InvalidOperationException">
    ///     Thrown if the requested version is older than <see cref="MinVersion" />.
    /// </exception>
    public IEnumerable<T> GetFrom(long version)
    {
        if (Count == 0 || version > MaxVersion)
            yield break;

        if (version < _minVersion)
            throw new InvalidOperationException(
                $"Requested version {version} is out of buffer range. MinVersion = {_minVersion}");

        for (var i = 0; i < Count; i++)
        {
            var index = (_head - Count + i + _buffer.Length) % _buffer.Length;
            if (_versions[index] >= version)
            {
                yield return _buffer[index];
            }
        }
    }
}