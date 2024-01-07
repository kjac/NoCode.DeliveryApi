using Umbraco.Cms.Core.Cache;

namespace Kjac.NoCode.DeliveryApi.Caching;

internal sealed class DistributedCacheRefresher : IDistributedCacheRefresher
{
    private readonly DistributedCache _distributedCache;
    private bool _suspended = false;

    public DistributedCacheRefresher(DistributedCache distributedCache)
        => _distributedCache = distributedCache;

    public void RefreshClientsCache()
        => RefreshIfNotSuspended(() => _distributedCache.RefreshAll(ClientsCacheRefresher.UniqueId));

    public void RefreshQueryCache()
        => RefreshIfNotSuspended(() => _distributedCache.RefreshAll(QueryCacheRefresher.UniqueId));

    public void Suspend()
        => _suspended = true;

    public void Resume()
        => _suspended = false;

    private void RefreshIfNotSuspended(Action refresh)
    {
        if (_suspended is true)
        {
            return;
        }

        refresh();
    }
}
