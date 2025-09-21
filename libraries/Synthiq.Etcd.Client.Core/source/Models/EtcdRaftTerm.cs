using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Synthiq.Etcd.Client;

[DebuggerDisplay("{ToString(),nq}")]
public readonly record struct EtcdRaftTerm : IComparable<EtcdRaftTerm>, IComparable
{
    private readonly UInt64 _value;

    public Boolean IsNonZero => this != Zero;
    public Boolean IsZero => this == Zero;

    [CLSCompliant(false)]
    public UInt64 Value => this._value;

    internal EtcdRaftTerm(UInt64 value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, UInt64.MinValue);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, UInt64.MaxValue);

        this._value = value;
    }

    public static EtcdRaftTerm Zero => default;
    public static EtcdRaftTerm MaxValue => new(UInt64.MaxValue);
    public static EtcdRaftTerm MinValue => Zero;

    /// <summary>
    /// Initializes a new instance of the <see cref="EtcdRaftTerm"/> structure
    /// to a specified term.
    /// </summary>
    ///
    /// <remarks>
    /// This method is an escape hatch; terms should usually not be created
    /// locally, with very few exceptions.
    /// Usage of this method is likely a code smell.
    /// </remarks>
    ///
    /// <param name="value">
    /// Term to initialize the structure with.
    /// </param>
    ///
    /// <returns>
    /// Returns an <see cref="EtcdRaftTerm"/> that represents a specific
    /// term of the Raft algorithm.
    /// </returns>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// The parameters specify an <see cref="EtcdRaftTerm"/> value less than
    /// <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
    /// </exception>
    [CLSCompliant(false)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static EtcdRaftTerm From(UInt64 value)
        => new(value);

    /// <summary>
    /// Initializes a new instance of the <see cref="EtcdRaftTerm"/> structure
    /// to a specified term.
    /// </summary>
    ///
    /// <remarks>
    /// This method is an escape hatch; terms should usually not be created
    /// locally, with very few exceptions.
    /// Usage of this method is likely a code smell.
    /// </remarks>
    ///
    /// <param name="value">
    /// Term to initialize the structure with.
    /// </param>
    ///
    /// <returns>
    /// Returns an <see cref="EtcdRaftTerm"/> that represents a specific
    /// term of the Raft algorithm.
    /// </returns>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// The parameters specify an <see cref="EtcdRaftTerm"/> value less than
    /// <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
    /// </exception>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static EtcdRaftTerm From(Int64 value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, 0L);

        return new((UInt64)value);
    }

    public static Boolean operator <(EtcdRaftTerm left, EtcdRaftTerm right)
        => left.Value < right.Value;

    public static Boolean operator <=(EtcdRaftTerm left, EtcdRaftTerm right)
        => left.Value <= right.Value;

    public static Boolean operator >(EtcdRaftTerm left, EtcdRaftTerm right)
        => left.Value > right.Value;

    public static Boolean operator >=(EtcdRaftTerm left, EtcdRaftTerm right)
        => left.Value >= right.Value;

    public Int32 CompareTo(EtcdRaftTerm other)
        => this.Value.CompareTo(other.Value);

    public Int32 CompareTo(Object? value)
        => value is null ? 1
            : value is EtcdRaftTerm other ? this.CompareTo(other)
            : throw new ArgumentException("Object must be of type EtcdRaftTerm.", nameof(value));

    public override String ToString()
        => $"{this.Value}";

    public Boolean TryGetInt64(out Int64 value)
    {
        if (this.Value <= Int64.MaxValue)
        {
            value = (Int64)this.Value;
            return true;
        }

        value = default;
        return false;
    }
}
