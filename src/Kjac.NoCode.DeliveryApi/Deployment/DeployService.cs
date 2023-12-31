using System.Text.Json;
using System.Text.Json.Serialization;
using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Extensions;
using Umbraco.Cms.Core.Sync;

namespace Kjac.NoCode.DeliveryApi.Deployment;

internal sealed class DeployService : IDeployService
{
    private readonly IFilterService _filterService;
    private readonly ISortService _sortService;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly IServerRoleAccessor _serverRoleAccessor;
    private readonly ILogger<DeployService> _logger;

    private const string DirectoryName = "NoCodeDeliveryApi";

    private const string FileName = "configuration.json";

    public DeployService(
        IFilterService filterService,
        ISortService sortService,
        IHostEnvironment hostEnvironment,
        IServerRoleAccessor serverRoleAccessor,
        ILogger<DeployService> logger)
    {
        _filterService = filterService;
        _sortService = sortService;
        _hostEnvironment = hostEnvironment;
        _serverRoleAccessor = serverRoleAccessor;
        _logger = logger;
    }

    public async Task ExportAsync()
    {
        if (CanDeploy() is false)
        {
            return;
        }

        IEnumerable<FilterModel> filters = await _filterService.GetAllAsync();
        IEnumerable<SortModel> sorts = await _sortService.GetAllAsync();
        var configDeployModel = new ConfigDeployModel
        {
            Filters = filters.Select(FilterDeployModel.FromFilterModel).ToArray(),
            Sorters = sorts.Select(SortDeployModel.FromSortModel).ToArray()
        };
        var configDeployData = JsonSerializer.Serialize(configDeployModel, SerializerOptions());

        try
        {
            Directory.CreateDirectory(GetDirectoryPath());
            await File.WriteAllTextAsync(GetFilePath(), configDeployData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not write {FileName} to disk", FileName);
        }
    }

    public async Task ImportAsync()
    {
        if (CanDeploy() is false)
        {
            return;
        }

        var filePath = GetFilePath();
        if (File.Exists(filePath) is false)
        {
            return;
        }

        ConfigDeployModel? configDeployModel = null;
        try
        {
            var configDeployData = await File.ReadAllTextAsync(filePath);
            configDeployModel = JsonSerializer.Deserialize<ConfigDeployModel>(configDeployData, SerializerOptions());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not read {FileName} from disk", FileName);
        }

        if (configDeployModel is null)
        {
            return;
        }

        await HandleImportAsync(
            (await _filterService.GetAllAsync()).ToArray(),
            configDeployModel.Filters,
            async key => await _filterService.DeleteAsync(key),
            async filter => await _filterService.AddAsync(filter.Name, filter.PropertyAliases.ToArray(), filter.FilterMatchType, filter.PrimitiveFieldType, filter.IndexFieldName),
            async (knownFilter, configFilter) =>
            {
                var changed = configFilter.Name != knownFilter.Name || configFilter.PropertyAliases.SequenceEqual(knownFilter.PropertyAliases) is false;
                if (changed)
                {
                    await _filterService.UpdateAsync(configFilter.Key, configFilter.Name, configFilter.PropertyAliases.ToArray());
                }
            }
        );

        await HandleImportAsync(
            (await _sortService.GetAllAsync()).ToArray(),
            configDeployModel.Sorters,
            async key => await _sortService.DeleteAsync(key),
            async sort => await _sortService.AddAsync(sort.Name, sort.PropertyAlias, sort.PrimitiveFieldType, sort.IndexFieldName),
            async (knownSort, configSort) =>
            {
                var changed = configSort.Name != knownSort.Name || configSort.PropertyAlias != knownSort.PropertyAlias;
                if (changed)
                {
                    await _sortService.UpdateAsync(configSort.Key, configSort.Name, configSort.PropertyAlias);
                }
            }
        );
    }

    private async Task HandleImportAsync<T, TD>(T[] known, TD[] config, Func<Guid, Task> deleteAsync,
        Func<TD, Task> addAsync, Func<T, TD, Task> updateAsync)
        where T : ModelBase
        where TD : DeployModelBase
    {
        var knownByKey = known.ToDictionary(k => k.Key);
        var configByKey = config.ToDictionary(c => c.Key);

        T[] obsolete = known.Where(k => configByKey.ContainsKey(k.Key) is false).ToArray();
        TD[] missing = config.Where(c => knownByKey.ContainsKey(c.Key) is false).ToArray();
        T[] current = known.Except(obsolete).ToArray();

        foreach (T modelBase in obsolete)
        {
            await deleteAsync(modelBase.Key);
        }

        foreach (TD modelBase in missing)
        {
            await addAsync(modelBase);
        }

        foreach (T modelBase in current)
        {
            await updateAsync(modelBase, configByKey[modelBase.Key]);
        }
    }

    private string GetDirectoryPath()
        => Path.Combine(_hostEnvironment.MapPathContentRoot(Umbraco.Cms.Core.Constants.SystemDirectories.Umbraco),
            DirectoryName);

    private string GetFilePath()
        => Path.Combine(GetDirectoryPath(), FileName);

    private bool CanDeploy()
        => _serverRoleAccessor.CurrentServerRole is not ServerRole.Subscriber;

    private JsonSerializerOptions SerializerOptions() => new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private class ConfigDeployModel
    {
        public required FilterDeployModel[] Filters { get; init; }

        // NOTE: it's called Sorters because that's how the UI mention Sorts :)
        public required SortDeployModel[] Sorters { get; init; }
    }

    private class FilterDeployModel : DeployModelBase
    {
        public required string[] PropertyAliases { get; init; }

        public required FilterMatchType FilterMatchType { get; init; }

        public static FilterDeployModel FromFilterModel(FilterModel filterModel) => new()
        {
            Key = filterModel.Key,
            Name = filterModel.Name,
            PropertyAliases = filterModel.PropertyAliases.ToArray(),
            FilterMatchType = filterModel.FilterMatchType,
            PrimitiveFieldType = filterModel.PrimitiveFieldType,
            IndexFieldName = filterModel.IndexFieldName
        };
    }

    private class SortDeployModel : DeployModelBase
    {
        public required string PropertyAlias { get; init; }

        public static SortDeployModel FromSortModel(SortModel sortModel) => new()
        {
            Key = sortModel.Key,
            Name = sortModel.Name,
            PropertyAlias = sortModel.PropertyAlias,
            PrimitiveFieldType = sortModel.PrimitiveFieldType,
            IndexFieldName = sortModel.IndexFieldName
        };
    }

    private abstract class DeployModelBase
    {
        public required Guid Key { get; init; }

        public required string Name { get; init; }

        public required PrimitiveFieldType PrimitiveFieldType { get; init; }

        public required string IndexFieldName { get; init; }
    }
}
