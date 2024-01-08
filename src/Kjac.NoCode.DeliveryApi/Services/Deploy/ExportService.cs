using Kjac.NoCode.DeliveryApi.Models;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core.Sync;

namespace Kjac.NoCode.DeliveryApi.Services.Deploy;

internal sealed class ExportService : DeployServiceBase, IExportService
{
    public ExportService(
        IServerRoleAccessor serverRoleAccessor,
        IHostEnvironment hostEnvironment,
        ILogger<ExportService> logger)
        : base(serverRoleAccessor, hostEnvironment, logger)
    {
    }

    public async Task ExportAsync(IEnumerable<FilterModel> filters)
    {
        if (CanDeploy() is false)
        {
            return;
        }

        FilterDeployModel[] deployModels = filters.Select(FilterDeployModel.FromFilterModel).ToArray();
        await SaveFilters(deployModels);
    }

    public async Task ExportAsync(IEnumerable<SortModel> sorts)
    {
        if (CanDeploy() is false)
        {
            return;
        }

        SortDeployModel[] deployModels = sorts.Select(SortDeployModel.FromSortModel).ToArray();
        await SaveSorts(deployModels);
    }
}
