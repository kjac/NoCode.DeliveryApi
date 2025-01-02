using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Services.Deploy;
using Umbraco.Cms.Core;

namespace Kjac.NoCode.DeliveryApi.Services;

internal sealed class SortServiceWithExport : ISortServiceWithExport
{
    private readonly ISortService _sortService;
    private readonly IExportService _exportService;

    public SortServiceWithExport(ISortService sortService, IExportService exportService)
    {
        _sortService = sortService;
        _exportService = exportService;
    }

    public async Task<IEnumerable<SortModel>> GetAllAsync()
        => await _sortService.GetAllAsync();

    public async Task<SortModel> GetAsync(string alias)
        => await _sortService.GetAsync(alias);

    public async Task<bool> ExistsAsync(string alias)
        => await _sortService.ExistsAsync(alias);

    public async Task<Attempt<OperationStatus>> AddAsync(
        string name,
        string propertyAlias,
        PrimitiveFieldType primitiveFieldType,
        Guid? key = null,
        string? indexFieldName = null)
        => await ExportOnSuccessAsync(async () => await _sortService.AddAsync(
            name,
            propertyAlias,
            primitiveFieldType,
            key,
            indexFieldName));

    public async Task<Attempt<OperationStatus>> UpdateAsync(Guid key, string name, string propertyAlias)
        => await ExportOnSuccessAsync(async () => await _sortService.UpdateAsync(key, name, propertyAlias));

    public async Task<Attempt<OperationStatus>> DeleteAsync(Guid key)
        => await ExportOnSuccessAsync(async () => await _sortService.DeleteAsync(key));

    public bool CanAdd()
        => _sortService.CanAdd();

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
