namespace Synthiq.Etcd.Client;

public static class EtcdTransportConfigurationBuilderGrpcExtensions
{
    public static IEtcdTransportConfigurationBuilder UseGrpc(this IEtcdTransportConfigurationBuilder builder)
    {
        return builder;
    }
}
