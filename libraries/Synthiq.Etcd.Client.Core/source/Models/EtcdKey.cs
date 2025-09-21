using System;
using System.Diagnostics;

namespace Synthiq.Etcd.Client;

[DebuggerDisplay("{ToString(),nq}")]
public readonly record struct EtcdKey : IComparable<EtcdKey>, IComparable
{
    private readonly ReadOnlyMemory<Byte> _bytes;

    public ReadOnlyMemory<Byte> Bytes => this._bytes;
    public ReadOnlySpan<Byte> Span => this._bytes.Span;

    public Boolean IsEmpty => this._bytes.IsEmpty;
    public Int32 Length => this._bytes.Length;

    internal EtcdKey(ReadOnlyMemory<Byte> bytes) => this._bytes = bytes;

    public static EtcdKey Empty => default;

    public static EtcdKey From(ReadOnlySpan<Byte> source)
    {
        if (source.IsEmpty) return Empty;

        var byteArray = GC.AllocateUninitializedArray<Byte>(source.Length);
        source.CopyTo(byteArray);

        return new EtcdKey(byteArray);
    }

    public static Boolean operator <(EtcdKey a, EtcdKey b)
        => a.CompareTo(b) < 0;

    public static Boolean operator >(EtcdKey a, EtcdKey b)
        => a.CompareTo(b) > 0;

    public static Boolean operator <=(EtcdKey a, EtcdKey b)
        => a.CompareTo(b) <= 0;

    public static Boolean operator >=(EtcdKey a, EtcdKey b)
        => a.CompareTo(b) >= 0;

    public Int32 CompareTo(EtcdKey other)
        => this.Span.SequenceCompareTo(other.Span);

    public Int32 CompareTo(Object? value)
        => value is null ? 1
            : value is EtcdKey other ? this.CompareTo(other)
            : throw new ArgumentException("Object must be of type EtcdKey.", nameof(value));

    public override String ToString()
        => $"Synthiq.Etcd.Client.EtcdKey[{this.Length}]";

    public Boolean EndsWith(ReadOnlySpan<Byte> suffix)
        => this.Span.EndsWith(suffix);

    public Boolean StartsWith(ReadOnlySpan<Byte> prefix)
        => this.Span.StartsWith(prefix);
}
