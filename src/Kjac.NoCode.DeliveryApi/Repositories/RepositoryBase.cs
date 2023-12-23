﻿using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Models.Dtos;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Extensions;

namespace Kjac.NoCode.DeliveryApi.Repositories;

internal abstract class RepositoryBase<TDto, TModel> : IRepositoryBase<TModel>
    where TDto : DtoBase, new()
    where TModel : ModelBase
{
    private readonly IScopeProvider _scopeProvider;
    private readonly IRuntimeState _runtimeState;

    // cache
    private readonly SemaphoreSlim _cacheLock = new SemaphoreSlim(1, 1);
    private List<TModel> _cache = new();
    private bool _cacheLoaded = false;

    protected RepositoryBase(IScopeProvider scopeProvider, IRuntimeState runtimeState)
    {
        _scopeProvider = scopeProvider;
        _runtimeState = runtimeState;
    }

    protected abstract TModel ParseModel(TDto dto);

    protected abstract TDto MapModelToDto(TModel model, TDto dto);
    
    public async Task<IEnumerable<TModel>> GetAllAsync()
    {
        await EnsureCache();
        return _cache.ToArray();
    }

    public async Task<TModel?> GetAsync(string alias)
    {
        await EnsureCache();
        return _cache.FirstOrDefault(model => model.Alias.InvariantEquals(alias));
    }
    
    public async Task<TModel?> GetAsync(Guid key)
    {
        await EnsureCache();
        return _cache.FirstOrDefault(model => model.Key == key);
    }

    public async Task<bool> CreateAsync(TModel model)
    {
        bool result;
        var dto = BuildDto(model);
        using (var scope = _scopeProvider.CreateScope())
        {
            result = await scope.Database.InsertAsync(dto) is not null;
            scope.Complete();
        }

        await EnsureCache(true);
        return result;
    }

    public async Task<bool> UpdateAsync(TModel model)
    {
        bool result;
        using (var scope = _scopeProvider.CreateScope())
        {
            var dto = await scope.Database.QueryAsync<TDto>(scope.Database.SqlContext.Sql()
                .Select<TDto>()
                .From<TDto>()
                .Where<TDto>(f => f.Key == model.Key)
            ).FirstOrDefaultAsync();

            if (dto is null)
            {
                return false;
            }

            MapModelToDto(model, dto);
            result = await scope.Database.UpdateAsync(dto) > 0;
            scope.Complete();
        }

        await EnsureCache(true);
        return result;
    }

    public async Task<bool> DeleteAsync(Guid key)
    {
        bool result;
        using (var scope = _scopeProvider.CreateScope())
        {
            var sql = scope.Database.SqlContext.Sql()
                .Delete<TDto>()
                .Where<TDto>(x => x.Key == key);

            result = await scope.Database.ExecuteAsync(sql) > 0;
            scope.Complete();
        }

        await EnsureCache(true);
        return result;
    }

    protected T ModelEnum<T>(string dtoEnumValue) where T : struct, Enum
        => Enum<T>.Parse(dtoEnumValue);

    protected string DtoEnum(Enum modelEnumValue)
        => modelEnumValue.ToString();

    private TDto BuildDto(TModel model)
        => MapModelToDto(model, new TDto());

    private async Task EnsureCache(bool forceReload = false)
    {
        bool IsLoaded() => forceReload is false && _cacheLoaded is true;

        if (IsLoaded())
        {
            return;
        }

        await _cacheLock.WaitAsync();
        try
        {
            if (IsLoaded())
            {
                return;
            }

            await LoadFromRepository();
            _cacheLoaded = true;
        }
        finally
        {
            _cacheLock.Release();
        }
    }
    
    private async Task LoadFromRepository()
    {
        try
        {
            using var scope = _scopeProvider.CreateScope();
            var dtos = await scope.Database.FetchAsync<TDto>();
            _cache = dtos?.Select(ParseModel).ToList() ?? [];
        }
        catch
        {
            // the Delivery API config attempts to load all configs at boot time; if we haven't run the migration yet,
            // this is expected to happen as the tables haven't been created at this point.
            if (_runtimeState.Level < RuntimeLevel.Run)
            {
                _cache = new List<TModel>();
                return;
            }

            throw;
        }
    }  
}