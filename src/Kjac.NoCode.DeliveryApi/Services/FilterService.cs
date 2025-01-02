using Kjac.NoCode.DeliveryApi.Caching;
using Kjac.NoCode.DeliveryApi.Extensions;
using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Repositories;
using Umbraco.Cms.Core;

namespace Kjac.NoCode.DeliveryApi.Services;

internal sealed class FilterService : QueryServiceBase<FilterModel>, IFilterService
{
    public FilterService(
        IFilterRepository repository,
        IFieldBufferService fieldBufferService,
        IModelAliasGenerator modelAliasGenerator,
        IDistributedCacheRefresher distributedCacheRefresher)
        : base(repository, fieldBufferService, modelAliasGenerator, distributedCacheRefresher)
    {
    }

    public async Task<Attempt<OperationStatus>> AddAsync(
        string name,
        string[] propertyAliases,
        FilterMatchType filterMatchType,
        PrimitiveFieldType primitiveFieldType,
        Guid? key = null,
        string? indexFieldName = null)
        => await AddAsync(
            primitiveFieldType.FilterFieldType(filterMatchType),
            primitiveFieldType,
            name,
            key,
            indexFieldName,
            filter =>
            {
                filter.PropertyAliases = propertyAliases;
                filter.FilterMatchType = filterMatchType;
            });

    public async Task<Attempt<OperationStatus>> UpdateAsync(Guid key, string name, string[] propertyAliases)
        => await UpdateAsync(key, name, filter => filter.PropertyAliases = propertyAliases);
}
