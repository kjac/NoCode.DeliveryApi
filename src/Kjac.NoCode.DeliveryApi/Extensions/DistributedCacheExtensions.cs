using Umbraco.Cms.Core.Cache;

namespace Kjac.NoCode.DeliveryApi.Caching;

internal static class DistributedCacheExtensions
{
    public static void RefreshClientsCache(this DistributedCache distributedCache)
        => distributedCache.RefreshAll(ClientsCacheRefresher.UniqueId);

    public static void RefreshQueryCache(this DistributedCache distributedCache)
        => distributedCache.RefreshAll(QueryCacheRefresher.UniqueId);
}
