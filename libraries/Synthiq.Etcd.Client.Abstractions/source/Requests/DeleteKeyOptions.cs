using System;

namespace Synthiq.Etcd.Client;

public readonly record struct DeleteKeyOptions
(
    Boolean ReturnPreviousKeyValue = false
);
