namespace Synthiq.Etcd.Client;

public sealed record PutKeyResponse
(
    ResponseHeader Header,
    EtcdKeyValue? PreviousKeyValue
) : IHasResponseHeader;
