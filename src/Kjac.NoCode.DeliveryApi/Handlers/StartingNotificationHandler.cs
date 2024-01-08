using Kjac.NoCode.DeliveryApi.Services.Deploy;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;

namespace Kjac.NoCode.DeliveryApi.Handlers;

internal sealed class StartingNotificationHandler : INotificationAsyncHandler<UmbracoApplicationStartingNotification>
{
    private readonly IRuntimeState _runtimeState;
    private readonly IImportService _importService;

    public StartingNotificationHandler(IRuntimeState runtimeState, IImportService importService)
    {
        _runtimeState = runtimeState;
        _importService = importService;
    }

    public async Task HandleAsync(UmbracoApplicationStartingNotification notification, CancellationToken cancellationToken)
    {
        if (_runtimeState.Level is not RuntimeLevel.Run)
        {
            return;
        }

        await _importService.ImportAsync();
    }
}
