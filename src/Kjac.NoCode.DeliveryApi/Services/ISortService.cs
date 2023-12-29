using Kjac.NoCode.DeliveryApi.Models;

namespace Kjac.NoCode.DeliveryApi.Services;

public interface ISortService
{
    Task<IEnumerable<SortModel>> GetAllAsync();

    Task<SortModel> GetAsync(string alias);

    Task<bool> ExistsAsync(string alias);

    Task<bool> AddAsync(
        string name,
        string propertyAlias,
        PrimitiveFieldType primitiveFieldType,
        string? indexFieldName = null);

    Task<bool> UpdateAsync(Guid key, string name, string propertyAlias);

    Task<bool> DeleteAsync(Guid key);

    bool CanAdd();
}
