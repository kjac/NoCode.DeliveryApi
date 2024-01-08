using Kjac.NoCode.DeliveryApi.Caching;
using Kjac.NoCode.DeliveryApi.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Sync;

namespace Kjac.NoCode.DeliveryApi.Services.Deploy;

internal sealed class ImportService : DeployServiceBase, IImportService
{
    private readonly IDistributedCacheRefresher _distributedCacheRefresher;
    private readonly IFilterService _filterService;
    private readonly ISortService _sortService;

    public ImportService(
        IServerRoleAccessor serverRoleAccessor,
        IHostEnvironment hostEnvironment,
        ILogger<ImportService> logger,
        IDistributedCacheRefresher distributedCacheRefresher,
        IFilterService filterService,
        ISortService sortService)
        : base(serverRoleAccessor, hostEnvironment, logger)
    {
        _distributedCacheRefresher = distributedCacheRefresher;
        _filterService = filterService;
        _sortService = sortService;
    }

    public async Task ImportAsync()
    {
        if (CanDeploy() is false)
        {
            return;
        }

        FilterDeployModel[]? filterDeployModels = await LoadFilters();
        SortDeployModel[]? sortDeployModels = await LoadSorts();

        if (filterDeployModels is null && sortDeployModels is null)
        {
            return;
        }

        try
        {
            _distributedCacheRefresher.Suspend();

            if (filterDeployModels is not null)
            {
                await HandleImportAsync(
                    (await _filterService.GetAllAsync()).ToArray(),
                    filterDeployModels,
                    async key => await _filterService.DeleteAsync(key),
                    async filter => await _filterService.AddAsync(
                        filter.Name,
                        filter.PropertyAliases.ToArray(),
                        filter.FilterMatchType,
                        filter.PrimitiveFieldType,
                        filter.Key,
                        filter.IndexFieldName),
                    async (knownFilter, configFilter) =>
                    {
                        var changed = configFilter.Name != knownFilter.Name
                                      || configFilter.PropertyAliases.SequenceEqual(knownFilter.PropertyAliases) is
                                          false;
                        if (changed)
                        {
                            await _filterService.UpdateAsync(
                                configFilter.Key,
                                configFilter.Name,
                                configFilter.PropertyAliases.ToArray());
                        }
                    }
                );
            }

            if (sortDeployModels is not null)
            {
                await HandleImportAsync(
                    (await _sortService.GetAllAsync()).ToArray(),
                    sortDeployModels,
                    async key => await _sortService.DeleteAsync(key),
                    async sort => await _sortService.AddAsync(
                        sort.Name,
                        sort.PropertyAlias,
                        sort.PrimitiveFieldType,
                        sort.Key,
                        sort.IndexFieldName),
                    async (knownSort, configSort) =>
                    {
                        var changed = configSort.Name != knownSort.Name
                                      || configSort.PropertyAlias != knownSort.PropertyAlias;
                        if (changed)
                        {
                            await _sortService.UpdateAsync(configSort.Key, configSort.Name, configSort.PropertyAlias);
                        }
                    }
                );
            }
        }
        finally
        {
            _distributedCacheRefresher.Resume();
            _distributedCacheRefresher.RefreshQueryCache();
        }
    }

    private async Task HandleImportAsync<T, TD>(
        T[] known,
        TD[] config,
        Func<Guid, Task> deleteAsync,
        Func<TD, Task> addAsync,
        Func<T, TD, Task> updateAsync)
        where T : ModelBase
        where TD : DeployModelBase
    {
        var knownByKey = known.ToDictionary(k => k.Key);
        var configByKey = config.ToDictionary(c => c.Key);

        T[] obsolete = known.Where(k => configByKey.ContainsKey(k.Key) is false).ToArray();
        TD[] missing = config.Where(c => knownByKey.ContainsKey(c.Key) is false).ToArray();
        T[] current = known.Except(obsolete).ToArray();

        foreach (T modelBase in obsolete)
        {
            await deleteAsync(modelBase.Key);
        }

        foreach (TD modelBase in missing)
        {
            await addAsync(modelBase);
        }

        foreach (T modelBase in current)
        {
            await updateAsync(modelBase, configByKey[modelBase.Key]);
        }
    }
}
