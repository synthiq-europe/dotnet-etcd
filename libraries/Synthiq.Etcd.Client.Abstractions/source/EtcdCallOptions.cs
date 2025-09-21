using System;

namespace Synthiq.Etcd.Client;

public readonly record struct EtcdCallOptions
(
    TimeSpan? Timeout = null
);
