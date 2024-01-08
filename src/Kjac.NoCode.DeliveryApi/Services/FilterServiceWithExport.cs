using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Services.Deploy;

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

    public async Task<bool> AddAsync(
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

    public async Task<bool> UpdateAsync(Guid key, string name, string[] propertyAliases)
        => await ExportOnSuccessAsync(async () => await _filterService.UpdateAsync(key, name, propertyAliases));

    public async Task<bool> DeleteAsync(Guid key)
        => await ExportOnSuccessAsync(async () => await _filterService.DeleteAsync(key));

    public bool CanAdd()
        => _filterService.CanAdd();

    private async Task<bool> ExportOnSuccessAsync(Func<Task<bool>> action)
    {
        var result = await action();
        if (result is true)
        {
            await _exportService.ExportAsync(await GetAllAsync());
        }

        return result;
    }
}
