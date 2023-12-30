using Kjac.NoCode.DeliveryApi.Caching;
using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Repositories;
using Umbraco.Cms.Core.Cache;

namespace Kjac.NoCode.DeliveryApi.Services;

internal class ClientService : IClientService
{
    private readonly IClientRepository _repository;
    private readonly DistributedCache _distributedCache;

    public ClientService(IClientRepository repository, DistributedCache distributedCache)
    {
        _repository = repository;
        _distributedCache = distributedCache;
    }

    public async Task<IEnumerable<ClientModel>> GetAllAsync()
        => await _repository.GetAllAsync();

    public async Task<bool> AddAsync(string name, string origin)
        => await RefreshCacheOnChange(async () => await _repository.CreateAsync(
            new ClientModel
            {
                Key = Guid.NewGuid(),
                Name = name,
                Origin = origin
            }));

    public async Task<bool> UpdateAsync(Guid key, string name, string origin)
    {
        ClientModel? model = await _repository.GetAsync(key);
        if (model is null)
        {
            return false;
        }

        model.Name = name;
        model.Origin = origin;

        return await RefreshCacheOnChange(() => _repository.UpdateAsync(model));
    }

    public async Task<bool> DeleteAsync(Guid key)
        => await RefreshCacheOnChange(async () => await _repository.DeleteAsync(key));

    private async Task<bool> RefreshCacheOnChange(Func<Task<bool>> action)
    {
        var result = await action();
        if (result is true)
        {
            _distributedCache.RefreshClientsCache();
        }

        return result;
    }
}
