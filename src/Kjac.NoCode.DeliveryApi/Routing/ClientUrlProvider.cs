using Kjac.NoCode.DeliveryApi.Extensions;
using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Services;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.Models.DeliveryApi;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Core.Web;
using Umbraco.Extensions;

namespace Kjac.NoCode.DeliveryApi.Routing;

public class ClientUrlProvider : IUrlProvider
{
    private readonly IClientService _clientService;
    private readonly IApiContentRouteBuilder _apiContentRouteBuilder;
    private readonly IUmbracoContextAccessor _umbracoContextAccessor;
    private readonly ILocalizationService _localizationService;

    public ClientUrlProvider(IClientService clientService, IApiContentRouteBuilder apiContentRouteBuilder, IUmbracoContextAccessor umbracoContextAccessor, ILocalizationService localizationService)
    {
        _clientService = clientService;
        _apiContentRouteBuilder = apiContentRouteBuilder;
        _umbracoContextAccessor = umbracoContextAccessor;
        _localizationService = localizationService;
    }

    public UrlInfo? GetUrl(IPublishedContent content, UrlMode mode, string? culture, Uri current)
        => null;

    public IEnumerable<UrlInfo> GetOtherUrls(int id, Uri current)
    {
        IPublishedContentCache? contentCache = _umbracoContextAccessor.GetRequiredUmbracoContext().Content;
        IPublishedContent? content = contentCache?.GetById(id);
        if (content is null)
        {
            return Enumerable.Empty<UrlInfo>();
        }

        ClientModel[] clients = _clientService.GetAllAsync().GetAwaiter().GetResult()
            .Where(client => client.PublishedUrlPath is not null)
            .ToArray();
        if (clients.Any() is false)
        {
            return Enumerable.Empty<UrlInfo>();
        }

        var variesByCulture = content.ContentType.VariesByCulture();
        var correctlyCasedCultures = _localizationService.GetAllLanguages().Select(language => language.IsoCode).ToArray();
        var cultures = variesByCulture
            ? correctlyCasedCultures.Where(content.Cultures.Keys.InvariantContains).ToArray()
            : correctlyCasedCultures;

        return cultures.SelectMany(culture =>
        {
            IApiContentRoute? route = _apiContentRouteBuilder.Build(content, variesByCulture ? culture : null);
            if (route is null)
            {
                return Enumerable.Empty<UrlInfo>();
            }

            ClientModel[] applicableClients = clients
                .Where(client => client.Culture is null || client.Culture.InvariantEquals(culture))
                .ToArray();

            return applicableClients.Select(client =>
            {
                var url = client.ParsePublishedUrl(content.Key, route, culture);
                return new UrlInfo(url, true, culture);
            }).ToArray();
        }).ToArray();
    }
}
