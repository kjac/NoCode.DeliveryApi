using Kjac.NoCode.DeliveryApi.Models;

namespace Kjac.NoCode.DeliveryApi.Services.Deploy;

internal interface IExportService
{
    Task ExportAsync(IEnumerable<FilterModel> filters);

    Task ExportAsync(IEnumerable<SortModel> sorts);
}
