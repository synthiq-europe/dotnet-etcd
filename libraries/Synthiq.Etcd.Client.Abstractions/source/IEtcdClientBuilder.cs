namespace Synthiq.Etcd.Client;

public interface IEtcdClientBuilder
{
    public IEtcdClient Build();

    public IEtcdClientBuilder WithTransports(IEtcdTransportConfiguration configuration);
}
