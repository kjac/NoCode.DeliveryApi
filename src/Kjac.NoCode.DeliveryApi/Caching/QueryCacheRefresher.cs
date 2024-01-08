using Kjac.NoCode.DeliveryApi.Repositories;
using Umbraco.Cms.Core.Cache;

namespace Kjac.NoCode.DeliveryApi.Caching;

internal sealed class QueryCacheRefresher : ICacheRefresher
{
    private readonly IFilterRepository _filterRepository;
    private readonly ISortRepository _sortRepository;

    public static readonly Guid UniqueId = Guid.Parse("1467C8BA-4ED8-42E8-B6E6-1FD8DECF4962");

    public QueryCacheRefresher(IFilterRepository filterRepository, ISortRepository sortRepository)
    {
        _filterRepository = filterRepository;
        _sortRepository = sortRepository;
    }

    public void RefreshAll()
    {
        _filterRepository.ReloadCache();
        _sortRepository.ReloadCache();
    }

    public void Refresh(int id) => throw new NotImplementedException();

    public void Remove(int id) => throw new NotImplementedException();

    public void Refresh(Guid id) => throw new NotImplementedException();

    public Guid RefresherUniqueId => UniqueId;

    public string Name => "No-Code Delivery API - Query Cache Refresher";
}
