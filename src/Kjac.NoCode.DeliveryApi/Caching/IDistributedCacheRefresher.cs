namespace Kjac.NoCode.DeliveryApi.Caching;

internal interface IDistributedCacheRefresher
{
    void RefreshClientsCache();

    void RefreshQueryCache();

    void Suspend();

    void Resume();
}
