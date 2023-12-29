using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Models.Dtos;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Extensions;

namespace Kjac.NoCode.DeliveryApi.Repositories;

internal abstract class QueryRepositoryBase<TDto, TModel> : RepositoryBase<TDto, TModel>, IQueryRepositoryBase<TModel>
    where TDto : QueryDtoBase, new()
    where TModel : QueryModelBase
{
    protected QueryRepositoryBase(IScopeProvider scopeProvider, IRuntimeState runtimeState)
        : base(scopeProvider, runtimeState)
    {
    }

    public async Task<TModel?> GetAsync(string alias)
    {
        await EnsureCache();
        return Cache.FirstOrDefault(model => model.Alias.InvariantEquals(alias));
    }
}
