﻿using Kjac.NoCode.DeliveryApi.Caching;
using Kjac.NoCode.DeliveryApi.Extensions;
using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Repositories;

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

    public async Task<bool> AddAsync(
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

    public async Task<bool> UpdateAsync(Guid key, string name, string propertyAlias)
        => await UpdateAsync(key, name, sort => sort.PropertyAlias = propertyAlias);
}
