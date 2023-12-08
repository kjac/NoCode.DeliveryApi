using Kjac.NoCode.DeliveryApi.Models;

namespace Kjac.NoCode.DeliveryApi.Repositories;

internal interface ISortRepository : IRepositoryBase<SortModel>
{
    Task<IEnumerable<SortModel>> GetAllAsync();

    Task<SortModel?> GetAsync(string alias);

    Task<SortModel?> GetAsync(Guid key);

    Task<bool> CreateAsync(SortModel model);

    Task<bool> UpdateAsync(SortModel model);

    Task<bool> DeleteAsync(Guid key);
}