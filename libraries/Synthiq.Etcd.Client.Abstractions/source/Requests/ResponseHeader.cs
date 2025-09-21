namespace Synthiq.Etcd.Client;

public readonly record struct ResponseHeader
(
    EtcdClusterId ClusterId,
    EtcdMemberId MemberId,
    EtcdRevision Revision,
    EtcdRaftTerm RaftTerm
);
