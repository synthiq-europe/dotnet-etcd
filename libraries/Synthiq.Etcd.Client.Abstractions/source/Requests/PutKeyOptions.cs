using System;

namespace Synthiq.Etcd.Client;

public readonly record struct PutKeyOptions
(
    EtcdLeaseId? LeaseId = null,
    Boolean RetainCurrentLease = false,
    Boolean RetainCurrentValue = false,
    Boolean ReturnPreviousKeyValue = false
);
