using Kjac.NoCode.DeliveryApi.Caching;
using Kjac.NoCode.DeliveryApi.Extensions;
using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Repositories;
using Umbraco.Cms.Core;

namespace Kjac.NoCode.DeliveryApi.Services;

internal sealed class SortService : QueryServiceBase<SortModel>, ISortService
{
    public SortService(
        ISortRepository repository,
        IFieldBufferService fieldBufferService,
        IModelAliasGenerator modelAliasGenerator,
        IDistributedCacheRefresher distributedCacheRefresher)
        : base(repository, fieldBufferService, modelAliasGenerator, distributedCacheRefresher)
    {
    }

    public async Task<Attempt<OperationStatus>> AddAsync(
        string name,
        string propertyAlias,
        PrimitiveFieldType primitiveFieldType,
        Guid? key = null,
        string? indexFieldName = null)
        => await AddAsync(
            primitiveFieldType.SortFieldType(),
            primitiveFieldType,
            name,
            key,
            indexFieldName,
            sort => sort.PropertyAlias = propertyAlias);

    public async Task<Attempt<OperationStatus>> UpdateAsync(Guid key, string name, string propertyAlias)
        => await UpdateAsync(key, name, sort => sort.PropertyAlias = propertyAlias);
}
