using System;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Net.Client;

namespace Synthiq.Etcd.Client.Transport.Grpc;

internal sealed class GrpcEtcdTransport : IEtcdTransport
{
    private readonly GrpcChannel _channel;

    private readonly Proto.Auth.AuthClient _authClient;
    private readonly Proto.Cluster.ClusterClient _clusterClient;
    private readonly Proto.Election.ElectionClient _electionClient;
    private readonly Proto.KV.KVClient _kvClient;
    private readonly Proto.Lease.LeaseClient _leaseClient;
    private readonly Proto.Lock.LockClient _lockClient;
    private readonly Proto.Maintenance.MaintenanceClient _maintenanceClient;
    private readonly Proto.Watch.WatchClient _watchClient;

    public GrpcEtcdTransport(GrpcChannel channel)
    {
        this._channel = channel;

        this._authClient = new(this._channel);
        this._clusterClient = new(this._channel);
        this._electionClient = new(this._channel);
        this._kvClient = new(this._channel);
        this._leaseClient = new(this._channel);
        this._lockClient = new(this._channel);
        this._maintenanceClient = new(this._channel);
        this._watchClient = new(this._channel);
    }
}
