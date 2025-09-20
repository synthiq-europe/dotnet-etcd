using System;
using System.ComponentModel;
using System.Diagnostics;

namespace Synthiq.Etcd.Client;

[DebuggerDisplay("{ToString(),nq}")]
public readonly record struct EtcdMemberId : IComparable<EtcdMemberId>, IComparable
{
    private readonly UInt64 _value;

    public Boolean IsNone => this == None;
    public Boolean IsSome => this != None;

    [CLSCompliant(false)]
    public UInt64 Value => this._value;

    internal EtcdMemberId(UInt64 value) => this._value = value;

    public static EtcdMemberId None => default;

    /// <summary>
    /// Initializes a new instance of the <see cref="EtcdMemberId"/> structure
    /// to a specified member ID.
    /// </summary>
    ///
    /// <remarks>
    /// This method is an escape hatch; IDs should usually not be created
    /// locally, with very few exceptions.
    /// Usage of this method is likely a code smell.
    /// </remarks>
    ///
    /// <param name="value">
    /// Member ID to initialize the structure with.
    /// </param>
    ///
    /// <returns>
    /// Returns an <see cref="EtcdMemberId"/> that represents a specific
    /// etcd member ID.
    /// </returns>
    ///
    /// <exception cref="ArgumentOutOfRangeException">
    /// The parameters specify an <see cref="EtcdMemberId"/> value equal to
    /// <see cref="None"/>.
    /// </exception>
    [CLSCompliant(false)]
    [EditorBrowsable(EditorBrowsableState.Advanced)]
    public static EtcdMemberId From(UInt64 value)
    {
        ArgumentOutOfRangeException.ThrowIfEqual(value, 0UL);

        return new(value);
    }

    public static Boolean operator <(EtcdMemberId left, EtcdMemberId right)
        => left.Value < right.Value;

    public static Boolean operator <=(EtcdMemberId left, EtcdMemberId right)
        => left.Value <= right.Value;

    public static Boolean operator >(EtcdMemberId left, EtcdMemberId right)
        => left.Value > right.Value;

    public static Boolean operator >=(EtcdMemberId left, EtcdMemberId right)
        => left.Value >= right.Value;

    public Int32 CompareTo(EtcdMemberId other)
        => this.Value.CompareTo(other.Value);

    public Int32 CompareTo(Object? value)
        => value is null ? 1
            : value is EtcdMemberId other ? this.CompareTo(other)
            : throw new ArgumentException($"Object must be of type EtcdMemberId.", nameof(value));

    public override String ToString()
        => this.IsNone ? nameof(None) : $"{this.Value:x16}";
}
