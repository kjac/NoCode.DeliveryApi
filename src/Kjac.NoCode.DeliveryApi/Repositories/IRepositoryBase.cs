using Kjac.NoCode.DeliveryApi.Models;

namespace Kjac.NoCode.DeliveryApi.Repositories;

internal interface IRepositoryBase<TModel>
    where TModel : ModelBase
{
    Task<IEnumerable<TModel>> GetAllAsync();

    Task<TModel?> GetAsync(Guid key);

    Task<bool> CreateAsync(TModel model);

    Task<bool> UpdateAsync(TModel model);

    Task<bool> DeleteAsync(Guid key);
}
