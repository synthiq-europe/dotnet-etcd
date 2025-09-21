namespace Synthiq.Etcd.Client;

public sealed record DeleteKeyResponse
(
    ResponseHeader Header,
    EtcdKeyValue? PreviousKeyValue
) : IHasResponseHeader;
