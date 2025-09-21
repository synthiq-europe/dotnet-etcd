using System.Threading;
using System.Threading.Tasks;

namespace Synthiq.Etcd.Client;

public interface IEtcdClient
{
    public Task<GetKeyResponse> GetKeyAsync(
        EtcdKey key,
        GetKeyOptions options = default,
        EtcdCallOptions? callOptions = null,
        CancellationToken cancellationToken = default);

    public Task<GetRangeResponse> GetKeyRangeAsync(
        EtcdKeyRange range,
        GetRangeOptions options = default,
        EtcdCallOptions? callOptions = null,
        CancellationToken cancellationToken = default);

    public Task<DeleteKeyResponse> DeleteKeyAsync(
        EtcdKey key,
        DeleteKeyOptions options = default,
        EtcdCallOptions? callOptions = null,
        CancellationToken cancellationToken = default);

    public Task<DeleteRangeResponse> DeleteRangeAsync(
        EtcdKeyRange range,
        DeleteRangeOptions options = default,
        EtcdCallOptions? callOptions = null,
        CancellationToken cancellationToken = default);

    public Task<PutKeyResponse> PutKeyAsync(
        EtcdKey key,
        PutKeyOptions options = default,
        EtcdCallOptions? callOptions = null,
        CancellationToken cancellationToken = default);
}
