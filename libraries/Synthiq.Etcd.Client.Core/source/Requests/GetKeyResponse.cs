using System;

namespace Synthiq.Etcd.Client;

public sealed record GetKeyResponse
(
    ResponseHeader Header,
    EtcdKeyValue? KeyValue,
    Boolean Exists
) : IHasResponseHeader;
