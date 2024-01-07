using Kjac.NoCode.DeliveryApi.Services;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.Services;

namespace Kjac.NoCode.DeliveryApi.Handlers;

internal sealed class StartedNotificationHandler : INotificationAsyncHandler<UmbracoApplicationStartedNotification>
{
    private readonly IRuntimeState _runtimeState;
    private readonly ICorsPolicyService _corsPolicyService;

    public StartedNotificationHandler(IRuntimeState runtimeState, ICorsPolicyService corsPolicyService)
    {
        _corsPolicyService = corsPolicyService;
        _runtimeState = runtimeState;
    }

    public async Task HandleAsync(UmbracoApplicationStartedNotification notification, CancellationToken cancellationToken)
    {
        if (_runtimeState.Level is not RuntimeLevel.Run)
        {
            return;
        }

        await _corsPolicyService.ApplyClientOriginsAsync();
    }
}
