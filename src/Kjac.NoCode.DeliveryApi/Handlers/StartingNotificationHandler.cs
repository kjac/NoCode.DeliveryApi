using Kjac.NoCode.DeliveryApi.Deployment;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;

namespace Kjac.NoCode.DeliveryApi.Handlers;

internal sealed class StartingNotificationHandler : INotificationAsyncHandler<UmbracoApplicationStartingNotification>
{
    private readonly IRuntimeState _runtimeState;
    private readonly IDeployService _deployService;

    public StartingNotificationHandler(IRuntimeState runtimeState, IDeployService deployService)
    {
        _runtimeState = runtimeState;
        _deployService = deployService;
    }

    public async Task HandleAsync(UmbracoApplicationStartingNotification notification, CancellationToken cancellationToken)
    {
        if (_runtimeState.Level is not RuntimeLevel.Run)
        {
            return;
        }

        await _deployService.ImportAsync();
    }
}
