using System;

namespace Synthiq.Etcd.Client;

public readonly record struct DeleteRangeOptions
(
    Boolean ReturnPreviousKeyValues = false
);
