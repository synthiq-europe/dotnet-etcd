using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Synthiq.Etcd.Client;

[DebuggerDisplay("{ToString(),nq}")]
public readonly record struct EtcdKeyVersion : IComparable<EtcdKeyVersion>, IComparable
{
    public Boolean IsNonZero => this != Zero;
    public Boolean IsZero => this == Zero;
    public Int64 Value { get; }

    internal EtcdKeyVersion(Int64 value)
    {
        ArgumentOutOfRangeException.ThrowIfLessThan(value, 0L);
        ArgumentOutOfRangeException.ThrowIfGreaterThan(value, Int64.MaxValue);

        this.Value = value;
    }

    public static EtcdKeyVersion Zero => default;
    public static EtcdKeyVersion MaxValue => new(Int64.MaxValue);
    public static EtcdKeyVersion MinValue => Zero;

    /// <summary>
    /// Initializes a new instance of the <see cref="EtcdKeyVersion"/> structure
    /// to a specified version.
    /// </summary>
    ///
    /// <remarks>
    /// This method is an escape hatch; versions should usually not be created
    /// locally, with very few exceptions.
    /// Usage of this method is likely a code smell.
    /// </remarks>
    ///
    /// <param name="value">
    /// Version to initialize the structure with.
    /// </param>
    ///
    /// <returns>
    /// Returns an <see cref="EtcdKeyVersion"/> that represents a specific
    /// version of a key.
    /// </returns>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// The parameters specify an <see cref="EtcdKeyVersion"/> value less than
    /// <see cref="MinValue"/> or greater than <see cref="MaxValue"/>.
    /// </exception>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static EtcdKeyVersion From(Int64 value)
        => new(value);

    public static Boolean operator <(EtcdKeyVersion left, EtcdKeyVersion right)
        => left.Value < right.Value;

    public static Boolean operator <=(EtcdKeyVersion left, EtcdKeyVersion right)
        => left.Value <= right.Value;

    public static Boolean operator >(EtcdKeyVersion left, EtcdKeyVersion right)
        => left.Value > right.Value;

    public static Boolean operator >=(EtcdKeyVersion left, EtcdKeyVersion right)
        => left.Value >= right.Value;

    public Int32 CompareTo(EtcdKeyVersion other)
        => this.Value.CompareTo(other.Value);

    public Int32 CompareTo(Object? value)
        => value is null ? 1
            : value is EtcdKeyVersion other ? this.CompareTo(other)
            : throw new ArgumentException("Object must be of type EtcdKeyVersion.", nameof(value));

    public override String ToString()
        => $"{this.Value}";
}
