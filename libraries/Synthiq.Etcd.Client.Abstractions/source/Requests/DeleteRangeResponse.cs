using System;
using System.Collections.Generic;

namespace Synthiq.Etcd.Client;

public sealed record DeleteRangeResponse
(
    ResponseHeader Header,
    IReadOnlyList<EtcdKeyValue> PreviousKeyValues,
    Int64 Deleted
) : IHasResponseHeader;
