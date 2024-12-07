using Kjac.NoCode.DeliveryApi.Models;
using Umbraco.Cms.Core;

namespace Kjac.NoCode.DeliveryApi.Services;

public interface ISortService
{
    Task<IEnumerable<SortModel>> GetAllAsync();

    Task<SortModel> GetAsync(string alias);

    Task<bool> ExistsAsync(string alias);

    Task<Attempt<OperationStatus>> AddAsync(
        string name,
        string propertyAlias,
        PrimitiveFieldType primitiveFieldType,
        Guid? key = null,
        string? indexFieldName = null);

    Task<Attempt<OperationStatus>> UpdateAsync(Guid key, string name, string propertyAlias);

    Task<Attempt<OperationStatus>> DeleteAsync(Guid key);

    bool CanAdd();
}
