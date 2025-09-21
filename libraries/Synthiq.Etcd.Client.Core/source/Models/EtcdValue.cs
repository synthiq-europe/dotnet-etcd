using System;
using System.Diagnostics;

namespace Synthiq.Etcd.Client;

[DebuggerDisplay("{ToString(),nq}")]
public readonly record struct EtcdValue : IComparable<EtcdValue>, IComparable
{
    private readonly ReadOnlyMemory<Byte> _bytes;

    public ReadOnlyMemory<Byte> Bytes => this._bytes;
    public ReadOnlySpan<Byte> Span => this._bytes.Span;

    public Boolean IsEmpty => this._bytes.IsEmpty;
    public Int32 Length => this._bytes.Length;

    internal EtcdValue(ReadOnlyMemory<Byte> bytes) => this._bytes = bytes;

    public static EtcdValue Empty => default;

    public static EtcdValue From(ReadOnlySpan<Byte> source)
    {
        if (source.IsEmpty) return Empty;

        var byteArray = GC.AllocateUninitializedArray<Byte>(source.Length);
        source.CopyTo(byteArray);

        return new EtcdValue(byteArray);
    }

    public static Boolean operator <(EtcdValue a, EtcdValue b)
        => a.CompareTo(b) < 0;

    public static Boolean operator >(EtcdValue a, EtcdValue b)
        => a.CompareTo(b) > 0;

    public static Boolean operator <=(EtcdValue a, EtcdValue b)
        => a.CompareTo(b) <= 0;

    public static Boolean operator >=(EtcdValue a, EtcdValue b)
        => a.CompareTo(b) >= 0;

    public Int32 CompareTo(EtcdValue other)
        => this.Span.SequenceCompareTo(other.Span);

    public Int32 CompareTo(Object? value)
        => value is null ? 1
            : value is EtcdValue other ? this.CompareTo(other)
            : throw new ArgumentException("Object must be of type EtcdValue.", nameof(value));

    public override String ToString()
        => $"Synthiq.Etcd.Client.EtcdValue[{this.Length}]";
}
