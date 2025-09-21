using System;
using System.Diagnostics;

namespace Synthiq.Etcd.Client;

/// <summary>
/// Represents an etcd key interval in lexicographic order using closed–open
/// semantics: <c>[Start, End)</c>.
/// </summary>
///
/// <remarks>
/// <para>
/// etcd range requests interpret <c>start</c> and <c>end</c> as a closed–open
/// interval: all keys <c>k</c> such that <c>Start ≤ k &amp;&amp; k &lt; End</c>.
/// </para>
///
/// <para>
/// When <see cref="HasEnd"/> is <c>false</c>, the interval is unbounded above and
/// denotes <c>[Start, +∞)</c>.
/// </para>
///
/// <para>
/// Keys are compared as raw byte sequences using etcd's lexicographic ordering.
/// No encoding is assumed.
/// </para>
/// </remarks>
[DebuggerDisplay("{ToString(),nq}")]
public readonly record struct EtcdKeyRange
{
    /// <summary>
    /// Inclusive lower bound of the key range.
    /// </summary>
    public EtcdKey Start { get; }

    /// <summary>
    /// Exclusive upper bound of the key range.
    /// Ignored when <see cref="HasEnd"/> is <c>false</c>.
    /// </summary>
    public EtcdKey End { get; }

    /// <summary>
    /// Value representing whether the key range has an exclusive upper bound.
    /// <c>false</c> means the range is unbounded above.
    /// </summary>
    public Boolean HasEnd { get; }

    /// <summary>
    /// Creates an <see cref="EtcdKeyRange"/>.
    /// </summary>
    ///
    /// <remarks>
    /// When <paramref name="hasEnd"/> is <c>true</c>, enforces
    /// <c>Start &lt; End</c> and non-empty <paramref name="end"/>.
    /// </remarks>
    ///
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="hasEnd"/> is <c>true</c> and
    /// <paramref name="end"/> is empty.
    /// </exception>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="hasEnd"/> is <c>true</c> and
    /// <paramref name="start"/> ≥ <paramref name="end"/>.
    /// </exception>
    internal EtcdKeyRange(EtcdKey start, EtcdKey end, Boolean hasEnd)
    {
        if (hasEnd)
        {
            if (end.IsEmpty)
                throw new ArgumentException("End must be non-empty when HasEnd is true.", nameof(end));

            if (start.CompareTo(end) >= 0)
                throw new ArgumentOutOfRangeException(nameof(end), "End must be greater than Start.");
        }

        this.Start = start;
        this.End = end;
        this.HasEnd = hasEnd;
    }

    /// <summary>
    /// Builds a range that matches all keys starting with
    /// <paramref name="prefix"/>.
    /// </summary>
    ///
    /// <remarks>
    /// <para>
    /// Computes the smallest key strictly greater than all keys with the given
    /// <paramref name="prefix"/>, and uses it as the exclusive
    /// <see cref="End"/>.
    /// </para>
    /// <para>
    /// If no such successor exists
    /// (e.g., <paramref name="prefix"/> is all <c>0xFF</c>), the returned range
    /// is unbounded above.
    /// </para>
    /// </remarks>
    ///
    /// <param name="prefix">
    /// The byte prefix to match.
    /// </param>
    ///
    /// <returns>
    /// An <see cref="EtcdKeyRange"/> representing
    /// <c>[prefix, end)</c> or
    /// <c>[prefix, +∞)</c>.
    /// </returns>
    public static EtcdKeyRange Prefix(EtcdKey prefix)
    {
        if (TryComputeEnd(prefix.Bytes.Span, out var end))
            return new(prefix, EtcdKey.From(end), true);

        return new(prefix, default, false);
    }

    /// <summary>
    /// Builds a range containing a single key.
    /// </summary>
    ///
    /// <remarks>
    /// Intended for APIs that route to a point GET. For a range scan of a single key using
    /// closed–open semantics, prefer <c>Span(key, key.Next())</c> if your code defines such an operation.
    /// </remarks>
    ///
    ///  <param name="key">
    /// The single key in the range.
    /// </param>
    public static EtcdKeyRange SingleKey(EtcdKey key)
        => new(key, EtcdKey.Empty, false);

    /// <summary>
    /// Builds a closed–open span <c>[start, end)</c>.
    /// </summary>
    ///
    /// <param name="start">
    /// Inclusive lower bound.
    /// </param>
    ///
    /// <param name="end">
    /// Exclusive upper bound.
    /// </param>
    ///
    /// <returns>
    /// The key range defined by <paramref name="start"/> and <paramref name="end"/>.
    /// </returns>
    ///
    /// <exception cref="ArgumentException">
    /// Thrown when <paramref name="end"/> is empty.
    /// </exception>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// Thrown when <paramref name="start"/> ≥ <paramref name="end"/>.
    /// </exception>
    public static EtcdKeyRange ClosedOpen(EtcdKey start, EtcdKey end)
        => new(start, end, true);

    private static Boolean TryComputeEnd(
        ReadOnlySpan<Byte> prefix,
        out ReadOnlySpan<Byte> end)
    {
        if (prefix.IsEmpty)
        {
            end = default;
            return false;
        }

        var byteArray = GC.AllocateUninitializedArray<Byte>(prefix.Length);
        prefix.CopyTo(byteArray);

        for (Int32 i = byteArray.Length - 1; i >= 0; --i)
        {
            if (byteArray[i] == 0xFF)
                continue;

            byteArray[i]++;
            end = byteArray.AsSpan(0, i + 1);

            return true;
        }

        end = default;
        return false;
    }
}
