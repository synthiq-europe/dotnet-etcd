namespace Synthiq.Etcd.Client;

public static class EtcdTransportConfigurationBuilderHttpExtensions
{
    public static IEtcdTransportConfigurationBuilder UseHttp(this IEtcdTransportConfigurationBuilder builder)
    {
        return builder;
    }
}
