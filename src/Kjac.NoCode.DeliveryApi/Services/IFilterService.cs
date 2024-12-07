using Kjac.NoCode.DeliveryApi.Models;
using Umbraco.Cms.Core;

namespace Kjac.NoCode.DeliveryApi.Services;

public interface IFilterService
{
    Task<IEnumerable<FilterModel>> GetAllAsync();

    Task<FilterModel> GetAsync(string alias);

    Task<bool> ExistsAsync(string alias);

    Task<Attempt<OperationStatus>> AddAsync(
        string name,
        string[] propertyAliases,
        FilterMatchType filterMatchType,
        PrimitiveFieldType primitiveFieldType,
        Guid? key = null,
        string? indexFieldName = null);

    Task<Attempt<OperationStatus>> UpdateAsync(Guid key, string name, string[] propertyAliases);

    Task<Attempt<OperationStatus>> DeleteAsync(Guid key);

    bool CanAdd();
}
