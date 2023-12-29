using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Repositories;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Extensions;

namespace Kjac.NoCode.DeliveryApi.Services;

internal abstract class QueryServiceBase<TModel> where TModel : QueryModelBase, new()
{
    private readonly IQueryRepositoryBase<TModel> _repository;
    private readonly IFieldBufferService _fieldBufferService;
    private readonly IModelAliasGenerator _modelAliasGenerator;

    protected QueryServiceBase(IQueryRepositoryBase<TModel> repository, IFieldBufferService fieldBufferService, IModelAliasGenerator modelAliasGenerator)
    {
        _repository = repository;
        _fieldBufferService = fieldBufferService;
        _modelAliasGenerator = modelAliasGenerator;
    }

    public async Task<IEnumerable<TModel>> GetAllAsync()
        => await _repository.GetAllAsync();

    public async Task<TModel> GetAsync(string alias)
        => await _repository.GetAsync(alias) ?? throw new ArgumentException($"The model {alias} is not defined. Use {nameof(ExistsAsync)} to determine if a model exists before attempting to access it.");

    public async Task<bool> ExistsAsync(string alias)
        => await _repository.GetAsync(alias) is not null;

    protected async Task<bool> AddAsync(
        FieldType fieldType,
        PrimitiveFieldType primitiveFieldType,
        string name,
        string? indexFieldName,
        Action<TModel> map)
    {
        indexFieldName ??= _fieldBufferService.GetField(fieldType)?.IndexFieldName;
        if (indexFieldName is null)
        {
            return false;
        }

        // alias must be unique
        var alias = _modelAliasGenerator.CreateAlias(name);
        if (await _repository.GetAsync(alias) is not null)
        {
            return false;
        }

        var model = new TModel
        {
            Key = Guid.NewGuid(),
            Name = name,
            Alias = alias,
            IndexFieldName = indexFieldName,
            PrimitiveFieldType = primitiveFieldType
        };
        map(model);

        return await _repository.CreateAsync(model);
    }

    protected async Task<bool> UpdateAsync(Guid key, string name, Action<TModel> map)
    {
        TModel? model = await _repository.GetAsync(key);
        if (model is null)
        {
            return false;
        }

        // alias must be unique
        var alias = _modelAliasGenerator.CreateAlias(name);
        if (model.Alias.InvariantEquals(alias) is false && await _repository.GetAsync(alias) is not null)
        {
            return false;
        }

        model.Name = name;
        model.Alias = alias;
        map(model);

        return await _repository.UpdateAsync(model);
    }

    public async Task<bool> DeleteAsync(Guid key)
        => await _repository.DeleteAsync(key);

    public bool CanAdd()
        => _fieldBufferService.IsDepleted() is false;
}
