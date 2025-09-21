using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Synthiq.Etcd.Client;

[DebuggerDisplay("{ToString(),nq}")]
public readonly record struct EtcdRevision : IComparable<EtcdRevision>, IComparable
{
    public Boolean IsNonZero => this != Zero;
    public Boolean IsZero => this == Zero;
    public Int64 Value { get; }

    internal EtcdRevision(Int64 value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, 0L);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, Int64.MaxValue);

        this.Value = value;
    }

    public static EtcdRevision Zero => default;
    public static EtcdRevision MaxValue => new(Int64.MaxValue);
    public static EtcdRevision MinValue => Zero;

    /// <summary>
    /// Initializes a new instance of the <see cref="EtcdRevision"/> structure
    /// to a specified revision.
    /// </summary>
    ///
    /// <remarks>
    /// This method is an escape hatch; revisions should usually not be created
    /// locally, with very few exceptions.
    /// Usage of this method is likely a code smell.
    /// </remarks>
    ///
    /// <param name="value">
    /// Revision to initialize the structure with.
    /// </param>
    ///
    /// <returns>
    /// Returns an <see cref="EtcdRevision"/> that represents a specific
    /// etcd revision.
    /// </returns>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// The parameters specify an <see cref="EtcdRevision"/> value less than
    /// <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
    /// </exception>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static EtcdRevision From(Int64 value)
        => new(value);

    public static Boolean operator <(EtcdRevision left, EtcdRevision right)
        => left.Value < right.Value;

    public static Boolean operator <=(EtcdRevision left, EtcdRevision right)
        => left.Value <= right.Value;

    public static Boolean operator >(EtcdRevision left, EtcdRevision right)
        => left.Value > right.Value;

    public static Boolean operator >=(EtcdRevision left, EtcdRevision right)
        => left.Value >= right.Value;

    public Int32 CompareTo(EtcdRevision other)
        => this.Value.CompareTo(other.Value);

    public Int32 CompareTo(Object? value)
        => value is null ? 1
            : value is EtcdRevision other ? this.CompareTo(other)
            : throw new ArgumentException("Object must be of type EtcdRevision.", nameof(value));

    public override String ToString()
        => $"{this.Value}";
}
