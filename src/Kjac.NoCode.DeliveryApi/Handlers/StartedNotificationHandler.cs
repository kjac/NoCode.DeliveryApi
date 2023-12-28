using Kjac.NoCode.DeliveryApi.Services;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Notifications;

namespace Kjac.NoCode.DeliveryApi.Handlers;

internal sealed class StartedNotificationHandler : INotificationAsyncHandler<UmbracoApplicationStartedNotification>
{
    private readonly ICorsPolicyService _corsPolicyService;

    public StartedNotificationHandler(ICorsPolicyService corsPolicyService)
        => _corsPolicyService = corsPolicyService;

    public async Task HandleAsync(UmbracoApplicationStartedNotification notification, CancellationToken cancellationToken)
        => await _corsPolicyService.ApplyClientOriginsAsync();
}