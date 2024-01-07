using Kjac.NoCode.DeliveryApi.Caching;
using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Repositories;

namespace Kjac.NoCode.DeliveryApi.Services;

internal class ClientService : IClientService
{
    private readonly IClientRepository _repository;
    private readonly IDistributedCacheRefresher _distributedCacheRefresher;

    public ClientService(IClientRepository repository, IDistributedCacheRefresher distributedCacheRefresher)
    {
        _repository = repository;
        _distributedCacheRefresher = distributedCacheRefresher;
    }

    public async Task<IEnumerable<ClientModel>> GetAllAsync()
        => await _repository.GetAllAsync();

    public async Task<bool> AddAsync(string name, string origin, string? previewUrlPath, string? publishedUrlPath, string? culture)
        => await RefreshCacheOnChange(async () => await _repository.CreateAsync(
            new ClientModel
            {
                Key = Guid.NewGuid(),
                Name = name,
                Origin = origin,
                PreviewUrlPath = previewUrlPath,
                PublishedUrlPath = publishedUrlPath,
                Culture = culture
            }));

    public async Task<bool> UpdateAsync(Guid key, string name, string origin, string? previewUrlPath, string? publishedUrlPath, string? culture)
    {
        ClientModel? model = await _repository.GetAsync(key);
        if (model is null)
        {
            return false;
        }

        model.Name = name;
        model.Origin = origin;
        model.PreviewUrlPath = previewUrlPath;
        model.PublishedUrlPath = publishedUrlPath;
        model.Culture = culture;

        return await RefreshCacheOnChange(() => _repository.UpdateAsync(model));
    }

    public async Task<bool> DeleteAsync(Guid key)
        => await RefreshCacheOnChange(async () => await _repository.DeleteAsync(key));

    private async Task<bool> RefreshCacheOnChange(Func<Task<bool>> action)
    {
        var result = await action();
        if (result is true)
        {
            _distributedCacheRefresher.RefreshClientsCache();
        }

        return result;
    }
}
