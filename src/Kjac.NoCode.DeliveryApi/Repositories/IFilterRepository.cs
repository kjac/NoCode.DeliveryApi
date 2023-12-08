using Kjac.NoCode.DeliveryApi.Models;

namespace Kjac.NoCode.DeliveryApi.Repositories;

internal interface IFilterRepository : IRepositoryBase<FilterModel>
{
    Task<IEnumerable<FilterModel>> GetAllAsync();

    Task<FilterModel?> GetAsync(string alias);

    Task<FilterModel?> GetAsync(Guid key);

    Task<bool> CreateAsync(FilterModel model);

    Task<bool> UpdateAsync(FilterModel model);

    Task<bool> DeleteAsync(Guid key);
}