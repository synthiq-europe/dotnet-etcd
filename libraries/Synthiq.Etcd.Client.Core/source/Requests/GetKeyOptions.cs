using System;

namespace Synthiq.Etcd.Client;

public readonly record struct GetKeyOptions
(
    EtcdConsistency Consistency = EtcdConsistency.Linearizable,
    Boolean ExistsOnly = false,
    Boolean KeyOnly = false,
    EtcdRevision? MinCreateRevision = null,
    EtcdRevision? MaxCreateRevision = null,
    EtcdRevision? MinModRevision = null,
    EtcdRevision? MaxModRevision = null
);
