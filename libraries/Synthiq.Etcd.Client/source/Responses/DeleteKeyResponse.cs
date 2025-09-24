using System;

namespace Synthiq.Etcd.Client;

public sealed record DeleteKeyResponse
(
    ResponseHeader Header,
    EtcdKeyValue? PreviousKeyValue,
    Boolean Deleted
);
