using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Synthiq.Etcd.Client;

[DebuggerDisplay("{ToString(),nq}")]
public readonly record struct EtcdLeaseId : IComparable<EtcdLeaseId>, IComparable
{
    public Boolean IsNone => this == None;
    public Boolean IsSome => this != None;
    public Int64 Value { get; }

    internal EtcdLeaseId(Int64 value) => this.Value = value;

    public static EtcdLeaseId None => default;

    /// <summary>
    /// Initializes a new instance of the <see cref="EtcdLeaseId"/> structure
    /// to a specified lease ID.
    /// </summary>
    ///
    /// <remarks>
    /// This method is an escape hatch; IDs should usually not be created
    /// locally, with very few exceptions.
    /// Usage of this method is likely a code smell.
    /// </remarks>
    ///
    /// <param name="value">
    /// Lease ID to initialize the structure with.
    /// </param>
    ///
    /// <returns>
    /// Returns an <see cref="EtcdLeaseId"/> that represents a specific
    /// etcd lease ID.
    /// </returns>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// The parameters specify an <see cref="EtcdLeaseId"/> value equal to
    /// <see cref="None"/>.
    /// </exception>
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static EtcdLeaseId From(Int64 value)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(value, 0L);

        return new(value);
    }

    public static Boolean operator <(EtcdLeaseId left, EtcdLeaseId right)
        => left.Value < right.Value;

    public static Boolean operator <=(EtcdLeaseId left, EtcdLeaseId right)
        => left.Value <= right.Value;

    public static Boolean operator >(EtcdLeaseId left, EtcdLeaseId right)
        => left.Value > right.Value;

    public static Boolean operator >=(EtcdLeaseId left, EtcdLeaseId right)
        => left.Value >= right.Value;

    public Int32 CompareTo(EtcdLeaseId other)
        => this.Value.CompareTo(other.Value);

    public Int32 CompareTo(Object? value)
        => value is null ? 1
            : value is EtcdLeaseId other ? this.CompareTo(other)
            : throw new ArgumentException($"Object must be of type EtcdLeaseId.", nameof(value));

    public override String ToString()
        => this.IsNone ? nameof(None) : $"{this.Value:x16}";
}
