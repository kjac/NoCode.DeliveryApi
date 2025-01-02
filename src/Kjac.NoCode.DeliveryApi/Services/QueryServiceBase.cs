using Kjac.NoCode.DeliveryApi.Caching;
using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Repositories;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Extensions;

namespace Kjac.NoCode.DeliveryApi.Services;

internal abstract class QueryServiceBase<TModel> where TModel : QueryModelBase, new()
{
    private readonly IQueryRepositoryBase<TModel> _repository;
    private readonly IFieldBufferService _fieldBufferService;
    private readonly IModelAliasGenerator _modelAliasGenerator;
    private readonly IDistributedCacheRefresher _distributedCacheRefresher;

    protected QueryServiceBase(
        IQueryRepositoryBase<TModel> repository,
        IFieldBufferService fieldBufferService,
        IModelAliasGenerator modelAliasGenerator,
        IDistributedCacheRefresher distributedCacheRefresher)
    {
        _repository = repository;
        _fieldBufferService = fieldBufferService;
        _modelAliasGenerator = modelAliasGenerator;
        _distributedCacheRefresher = distributedCacheRefresher;
    }

    public async Task<IEnumerable<TModel>> GetAllAsync()
        => await _repository.GetAllAsync();

    public async Task<TModel> GetAsync(string alias)
        => await _repository.GetAsync(alias) ?? throw new ArgumentException($"The model {alias} is not defined. Use {nameof(ExistsAsync)} to determine if a model exists before attempting to access it.");

    public async Task<bool> ExistsAsync(string alias)
        => await _repository.GetAsync(alias) is not null;

    protected async Task<Attempt<OperationStatus>> AddAsync(
        FieldType fieldType,
        PrimitiveFieldType primitiveFieldType,
        string name,
        Guid? key,
        string? indexFieldName,
        Action<TModel> map)
    {
        indexFieldName ??= _fieldBufferService.GetField(fieldType)?.IndexFieldName;
        if (indexFieldName is null)
        {
            return Attempt.Fail(OperationStatus.UnknownIndexFieldName);
        }

        // alias must be unique
        var alias = _modelAliasGenerator.CreateAlias(name);
        if (await _repository.GetAsync(alias) is not null)
        {
            return Attempt.Fail(OperationStatus.DuplicateAlias);
        }

        var model = new TModel
        {
            Key = key ?? Guid.NewGuid(),
            Name = name,
            Alias = alias,
            IndexFieldName = indexFieldName,
            PrimitiveFieldType = primitiveFieldType
        };
        map(model);

        var created = await RefreshCacheOnChange(async () => await _repository.CreateAsync(model));
        return created
            ? Attempt.Succeed(OperationStatus.Success)
            : Attempt.Fail(OperationStatus.FailedCreate);
    }

    protected async Task<Attempt<OperationStatus>> UpdateAsync(Guid key, string name, Action<TModel> map)
    {
        TModel? model = await _repository.GetAsync(key);
        if (model is null)
        {
            return Attempt.Fail(OperationStatus.NotFound);
        }

        // alias must be unique
        var alias = _modelAliasGenerator.CreateAlias(name);
        if (model.Alias.InvariantEquals(alias) is false && await _repository.GetAsync(alias) is not null)
        {
            return Attempt.Fail(OperationStatus.DuplicateAlias);
        }

        model.Name = name;
        model.Alias = alias;
        map(model);

        var updated = await RefreshCacheOnChange(async () => await _repository.UpdateAsync(model));
        return updated
            ? Attempt.Succeed(OperationStatus.Success)
            : Attempt.Fail(OperationStatus.FailedUpdate);
    }

    public async Task<Attempt<OperationStatus>> DeleteAsync(Guid key)
    {
        var deleted = await RefreshCacheOnChange(async () => await _repository.DeleteAsync(key));
        return deleted
            ? Attempt.Succeed(OperationStatus.Success)
            : Attempt.Fail(OperationStatus.FailedDelete);
    }

    public bool CanAdd()
        => _fieldBufferService.IsDepleted() is false;

    private async Task<bool> RefreshCacheOnChange(Func<Task<bool>> action)
    {
        var result = await action();
        if (result is true)
        {
            _distributedCacheRefresher.RefreshQueryCache();
        }

        return result;
    }
}
