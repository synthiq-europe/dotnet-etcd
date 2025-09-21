using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Synthiq.Etcd.Client;

/// <summary>
/// etcd key/value with immutable views and revision metadata.
/// </summary>
[DebuggerDisplay("{ToString(),nq}")]
public readonly record struct EtcdKeyValue : IComparable<EtcdKeyValue>, IComparable
{
    public EtcdKey Key { get; }
    public EtcdValue Value { get; }
    public EtcdRevision CreateRevision { get; }
    public EtcdRevision ModRevision { get; }
    public EtcdKeyVersion Version { get; }
    public EtcdLeaseId LeaseId { get; }

    internal EtcdKeyValue(
        EtcdKey key,
        EtcdValue value,
        EtcdRevision createRevision,
        EtcdRevision modRevision,
        EtcdKeyVersion version,
        EtcdLeaseId leaseId)
    {
        this.Key = key;
        this.Value = value;
        this.CreateRevision = createRevision;
        this.ModRevision = modRevision;
        this.Version = version;
        this.LeaseId = leaseId;
    }

    public static Boolean operator <(EtcdKeyValue left, EtcdKeyValue right)
        => left.CompareTo(right) < 0;

    public static Boolean operator <=(EtcdKeyValue left, EtcdKeyValue right)
        => left.CompareTo(right) <= 0;

    public static Boolean operator >(EtcdKeyValue left, EtcdKeyValue right)
        => left.CompareTo(right) > 0;

    public static Boolean operator >=(EtcdKeyValue left, EtcdKeyValue right)
        => left.CompareTo(right) >= 0;

    public Int32 CompareTo(EtcdKeyValue other)
    {
        Int32 c = Comparer<EtcdKey>.Default.Compare(this.Key, other.Key);
        if (c != 0) return c;

        return Comparer<EtcdRevision>.Default.Compare(this.ModRevision, other.ModRevision);
    }

    public Int32 CompareTo(Object? obj)
        => obj is null ? 1
            : obj is EtcdKeyValue kv ? this.CompareTo(kv)
            : throw new ArgumentException("Object must be of type EtcdKeyValue.", nameof(obj));

    public Boolean Equals(EtcdKeyValue other)
        => this.Key.Equals(other.Key) && this.ModRevision.Equals(other.ModRevision);

    public override Int32 GetHashCode()
        => HashCode.Combine(this.Key, this.ModRevision);
}
