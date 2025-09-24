using System;

namespace Synthiq.Etcd.Client;

public readonly record struct GetRangeOptions
(
    EtcdConsistency Consistency = EtcdConsistency.Linearizable,
    Boolean CountOnly = false,
    Boolean KeysOnly = false,
    EtcdRevision? MinCreateRevision = null,
    EtcdRevision? MaxCreateRevision = null,
    EtcdRevision? MinModRevision = null,
    EtcdRevision? MaxModRevision = null,
    Int64 Limit = 0,
    EtcdSortOrder SortOrder = EtcdSortOrder.None,
    EtcdSortTarget SortTarget = EtcdSortTarget.Key
);
