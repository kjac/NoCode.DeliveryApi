using Kjac.NoCode.DeliveryApi.Models;

namespace Kjac.NoCode.DeliveryApi.Services;

public interface IFilterService
{
    Task<IEnumerable<FilterModel>> GetAllAsync();
    
    Task<FilterModel> GetAsync(string alias);

    Task<bool> ExistsAsync(string alias);

    Task<bool> AddAsync(string name, string[] propertyAliases, FilterMatchType filterMatchType, PrimitiveFieldType primitiveFieldType, string? indexFieldName = null);

    Task<bool> UpdateAsync(Guid key, string name, string[] propertyAliases);

    Task<bool> DeleteAsync(Guid key);

    bool CanAdd();
}