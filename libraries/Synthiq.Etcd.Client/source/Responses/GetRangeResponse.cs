using System;
using System.Collections.Generic;

namespace Synthiq.Etcd.Client;

public sealed record GetRangeResponse
(
    ResponseHeader Header,
    IReadOnlyList<EtcdKeyValue> KeyValues,
    Int64 Count,
    Boolean HasMore
);
