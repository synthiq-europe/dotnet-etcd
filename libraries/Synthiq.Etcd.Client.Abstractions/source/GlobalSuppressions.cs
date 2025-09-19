using System.Diagnostics.CodeAnalysis;

[assembly: SuppressMessage("Design", "CA1040:Avoid empty interfaces",
    Justification = "This type identifies transport configurations at compile time.",
    Scope = "type",
    Target = "~T:Synthiq.Etcd.Client.IEtcdTransportConfiguration")]
