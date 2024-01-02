using Kjac.NoCode.DeliveryApi.Extensions;
using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Services;
using Microsoft.AspNetCore.Http;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.Events;
using Umbraco.Cms.Core.Models.ContentEditing;
using Umbraco.Cms.Core.Models.DeliveryApi;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.Notifications;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Extensions;

namespace Kjac.NoCode.DeliveryApi.Handlers;

internal class SendingContentNotificationHandler : INotificationAsyncHandler<SendingContentNotification>
{
    private readonly IApiContentRouteBuilder _apiContentRouteBuilder;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IClientService _clientService;

    public SendingContentNotificationHandler(
        IApiContentRouteBuilder apiContentRouteBuilder,
        IHttpContextAccessor httpContextAccessor,
        IClientService clientService)
    {
        _apiContentRouteBuilder = apiContentRouteBuilder;
        _httpContextAccessor = httpContextAccessor;
        _clientService = clientService;
    }

    public async Task HandleAsync(SendingContentNotification notification, CancellationToken cancellationToken)
    {
        if (notification.Content.Key.HasValue is false || notification.UmbracoContext.Content is null)
        {
            return;
        }

        IApiContentRoute? route = GetRoute(notification.Content.Key.Value, notification.UmbracoContext.Content);
        if (route is null)
        {
            return;
        }

        ClientModel[] previewClients = (await _clientService.GetAllAsync())
            .Where(client => client.PreviewUrlPath.IsNullOrWhiteSpace() is false)
            .ToArray();
        if (previewClients.Any() is false)
        {
            return;
        }

        foreach (ContentVariantDisplay variantDisplay in notification.Content.Variants)
        {
            ClientModel[] applicableClients = previewClients.Where(client
                => client.Culture is null
                   || variantDisplay.Language is null
                   || client.Culture.InvariantEquals(variantDisplay.Language.IsoCode))
                .ToArray();

            variantDisplay.AdditionalPreviewUrls = applicableClients.Select(client => new NamedUrl
            {
                Name = client.Name,
                Url = client.ParsePreviewUrl(notification.Content.Key.Value, route, variantDisplay.Language?.IsoCode)
            }).ToArray();
        }
    }

    private IApiContentRoute? GetRoute(Guid contentKey, IPublishedContentCache publishedContentCache)
    {
        IPublishedContent? content = publishedContentCache.GetById(true, contentKey);
        if (content is null)
        {
            return null;
        }

        HttpContext? context = _httpContextAccessor.HttpContext;
        if (context is null)
        {
            return null;
        }

        // we need to set this for the API route builder to handle unpublished draft content correctly
        context.Request.Headers["Preview"] = "true";
        return _apiContentRouteBuilder.Build(content);
    }
}
