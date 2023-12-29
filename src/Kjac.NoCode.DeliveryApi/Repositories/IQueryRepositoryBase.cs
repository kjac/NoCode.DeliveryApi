using Kjac.NoCode.DeliveryApi.Models;

namespace Kjac.NoCode.DeliveryApi.Repositories;

internal interface IQueryRepositoryBase<TModel> : IRepositoryBase<TModel>
    where TModel : QueryModelBase
{
    Task<TModel?> GetAsync(string alias);
}
