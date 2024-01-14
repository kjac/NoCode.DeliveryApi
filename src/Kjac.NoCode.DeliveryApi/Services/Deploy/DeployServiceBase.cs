using System.Text.Json;
using System.Text.Json.Serialization;
using Kjac.NoCode.DeliveryApi.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Extensions;
using Umbraco.Cms.Core.Sync;

namespace Kjac.NoCode.DeliveryApi.Services.Deploy;

internal abstract class DeployServiceBase
{
    private readonly IServerRoleAccessor _serverRoleAccessor;
    private readonly IHostEnvironment _hostEnvironment;
    private readonly ILogger _logger;

    private const string DirectoryName = "NoCode\\DeliveryApi";
    private const string FiltersFileName = "filters.json";
    private const string SortsFileName = "sorters.json";

    protected DeployServiceBase(IServerRoleAccessor serverRoleAccessor, IHostEnvironment hostEnvironment, ILogger logger)
    {
        _serverRoleAccessor = serverRoleAccessor;
        _hostEnvironment = hostEnvironment;
        _logger = logger;
    }

    protected bool CanDeploy()
        => _serverRoleAccessor.CurrentServerRole is not ServerRole.Subscriber;

    protected async Task SaveFilters(IEnumerable<FilterDeployModel> models)
        => await Save(models, FiltersFileName);

    protected async Task SaveSorts(IEnumerable<SortDeployModel> models)
        => await Save(models, SortsFileName);

    protected async Task<FilterDeployModel[]?> LoadFilters()
        => await Load<FilterDeployModel>(FiltersFileName);

    protected async Task<SortDeployModel[]?> LoadSorts()
        => await Load<SortDeployModel>(SortsFileName);

    private async Task Save<T>(IEnumerable<T> models, string fileName)
        where T : DeployModelBase
    {
        var deployData = JsonSerializer.Serialize(models, SerializerOptions());

        try
        {
            Directory.CreateDirectory(GetDirectoryPath());
            await File.WriteAllTextAsync(GetFilePath(fileName), deployData);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not write {FileName} to disk", fileName);
        }
    }

    private async Task<T[]?> Load<T>(string fileName)
        where T : DeployModelBase
    {
        var filePath = GetFilePath(fileName);
        if (File.Exists(filePath) is false)
        {
            return null;
        }

        try
        {
            var configDeployData = await File.ReadAllTextAsync(filePath);
            return JsonSerializer.Deserialize<T[]>(configDeployData, SerializerOptions());
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Could not read {FileName} from disk", fileName);
            return null;
        }
    }

    private JsonSerializerOptions SerializerOptions() => new()
    {
        WriteIndented = true,
        Converters = { new JsonStringEnumConverter() },
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    private string GetDirectoryPath()
        => _hostEnvironment.MapPathContentRoot(DirectoryName);

    private string GetFilePath(string fileName)
        => Path.Combine(GetDirectoryPath(), fileName);

    protected class FilterDeployModel : DeployModelBase
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

    protected class SortDeployModel : DeployModelBase
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

    protected abstract class DeployModelBase
    {
        public required Guid Key { get; init; }

        public required string Name { get; init; }

        public required PrimitiveFieldType PrimitiveFieldType { get; init; }

        public required string IndexFieldName { get; init; }
    }
}
