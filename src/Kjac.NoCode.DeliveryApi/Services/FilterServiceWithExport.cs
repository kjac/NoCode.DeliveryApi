using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Services.Deploy;
using Umbraco.Cms.Core;

namespace Kjac.NoCode.DeliveryApi.Services;

internal sealed class FilterServiceWithExport : IFilterServiceWithExport
{
    private readonly IFilterService _filterService;
    private readonly IExportService _exportService;

    public FilterServiceWithExport(IFilterService filterService, IExportService exportService)
    {
        _filterService = filterService;
        _exportService = exportService;
    }

    public async Task<IEnumerable<FilterModel>> GetAllAsync()
        => await _filterService.GetAllAsync();

    public async Task<FilterModel> GetAsync(string alias)
        => await _filterService.GetAsync(alias);

    public async Task<bool> ExistsAsync(string alias)
        => await _filterService.ExistsAsync(alias);

    public async Task<Attempt<OperationStatus>> AddAsync(
        string name,
        string[] propertyAliases,
        FilterMatchType filterMatchType,
        PrimitiveFieldType primitiveFieldType,
        Guid? key = null,
        string? indexFieldName = null)
        => await ExportOnSuccessAsync(async () => await _filterService.AddAsync(
            name,
            propertyAliases,
            filterMatchType,
            primitiveFieldType,
            key,
            indexFieldName));

    public async Task<Attempt<OperationStatus>> UpdateAsync(Guid key, string name, string[] propertyAliases)
        => await ExportOnSuccessAsync(async () => await _filterService.UpdateAsync(key, name, propertyAliases));

    public async Task<Attempt<OperationStatus>> DeleteAsync(Guid key)
        => await ExportOnSuccessAsync(async () => await _filterService.DeleteAsync(key));

    public bool CanAdd()
        => _filterService.CanAdd();

    private async Task<Attempt<OperationStatus>> ExportOnSuccessAsync(Func<Task<Attempt<OperationStatus>>> action)
    {
        Attempt<OperationStatus> result = await action();
        if (result.Success)
        {
            await _exportService.ExportAsync(await GetAllAsync());
        }

        return result;
    }
}
