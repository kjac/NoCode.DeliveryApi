using Kjac.NoCode.DeliveryApi.Extensions;
using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Repositories;

namespace Kjac.NoCode.DeliveryApi.Services;

internal class FilterService : ServiceBase<FilterModel>, IFilterService
{
    public FilterService(IFilterRepository repository, IFieldBufferService fieldBufferService, IModelAliasGenerator modelAliasGenerator)
        : base(repository, fieldBufferService, modelAliasGenerator)
    {
    }

    public async Task<bool> AddAsync(string name, string[] propertyAliases, FilterMatchType filterMatchType, PrimitiveFieldType primitiveFieldType)
        => await AddAsync(primitiveFieldType.FilterFieldType(filterMatchType), primitiveFieldType, name, filter =>
        {
            filter.PropertyAliases = propertyAliases;
            filter.FilterMatchType = filterMatchType;
        });

    public async Task<bool> UpdateAsync(Guid key, string name, string[] propertyAliases)
        => await UpdateAsync(key, name, filter => filter.PropertyAliases = propertyAliases);
}