using Kjac.NoCode.DeliveryApi.Caching;
using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Repositories;
using Umbraco.Cms.Core;

namespace Kjac.NoCode.DeliveryApi.Services;

internal sealed class ClientService : IClientService
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

    public async Task<Attempt<OperationStatus>> AddAsync(string name, string origin, string? previewUrlPath, string? publishedUrlPath, string? culture)
        => await RefreshCacheOnChange(async () =>
        {
            var created = await _repository.CreateAsync(
                new ClientModel
                {
                    Key = Guid.NewGuid(),
                    Name = name,
                    Origin = origin,
                    PreviewUrlPath = previewUrlPath,
                    PublishedUrlPath = publishedUrlPath,
                    Culture = culture
                });

            return created
                ? Attempt.Succeed(OperationStatus.Success)
                : Attempt.Fail(OperationStatus.FailedCreate);
        });

    public async Task<Attempt<OperationStatus>> UpdateAsync(Guid key, string name, string origin, string? previewUrlPath, string? publishedUrlPath, string? culture)
    {
        ClientModel? model = await _repository.GetAsync(key);
        if (model is null)
        {
            return Attempt.Fail(OperationStatus.NotFound);
        }

        model.Name = name;
        model.Origin = origin;
        model.PreviewUrlPath = previewUrlPath;
        model.PublishedUrlPath = publishedUrlPath;
        model.Culture = culture;

        return await RefreshCacheOnChange(async () =>
        {
            var updated = await _repository.UpdateAsync(model);
            return updated
                ? Attempt.Succeed(OperationStatus.Success)
                : Attempt.Fail(OperationStatus.FailedUpdate);
        });
    }

    public async Task<Attempt<OperationStatus>> DeleteAsync(Guid key)
        => await RefreshCacheOnChange(async () =>
        {
            var deleted = await _repository.DeleteAsync(key);
            return deleted
                ? Attempt.Succeed(OperationStatus.Success)
                : Attempt.Fail(OperationStatus.FailedDelete);
        });

    private async Task<Attempt<OperationStatus>> RefreshCacheOnChange(Func<Task<Attempt<OperationStatus>>> action)
    {
        Attempt<OperationStatus> result = await action();
        if (result.Success)
        {
            _distributedCacheRefresher.RefreshClientsCache();
        }

        return result;
    }
}
