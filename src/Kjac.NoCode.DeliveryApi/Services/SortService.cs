﻿using Kjac.NoCode.DeliveryApi.Extensions;
using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Repositories;

namespace Kjac.NoCode.DeliveryApi.Services;

internal class SortService : QueryServiceBase<SortModel>, ISortService
{
    public SortService(ISortRepository repository, IFieldBufferService fieldBufferService,
        IModelAliasGenerator modelAliasGenerator)
        : base(repository, fieldBufferService, modelAliasGenerator)
    {
    }

    public async Task<bool> AddAsync(
        string name,
        string propertyAlias,
        PrimitiveFieldType primitiveFieldType,
        string? indexFieldName = null)
        => await AddAsync(
            primitiveFieldType.SortFieldType(),
            primitiveFieldType,
            name,
            indexFieldName,
            sort => sort.PropertyAlias = propertyAlias);

    public async Task<bool> UpdateAsync(Guid key, string name, string propertyAlias)
        => await UpdateAsync(key, name, sort => sort.PropertyAlias = propertyAlias);
}
