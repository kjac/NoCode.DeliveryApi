using System.Text.RegularExpressions;
using Kjac.NoCode.DeliveryApi.Models;
using Umbraco.Cms.Core.Models.DeliveryApi;

namespace Kjac.NoCode.DeliveryApi.Extensions;

internal static partial class ClientModelExtensions
{
    public static string ParsePreviewUrl(this ClientModel clientModel, Guid contentKey, IApiContentRoute contentRoute, string? culture)
    {
        var previewUrlPath = clientModel.PreviewUrlPath ?? throw new ArgumentException("Preview URL path cannot be null here", nameof(clientModel));
        return ParseUrlPath(clientModel, contentKey, contentRoute, culture, previewUrlPath);
    }

    public static string ParsePublishedUrl(this ClientModel clientModel, Guid contentKey, IApiContentRoute contentRoute, string? culture)
    {
        var publishedUrlPath = clientModel.PublishedUrlPath ?? throw new ArgumentException("Published URL path cannot be null here", nameof(clientModel));
        return ParseUrlPath(clientModel, contentKey, contentRoute, culture, publishedUrlPath);
    }

    private static string ParseUrlPath(ClientModel clientModel, Guid contentKey, IApiContentRoute contentRoute, string? culture, string urlPath)
    {
        var parsedUrlPath = PathPlaceholderParserRegex().Replace(urlPath,
            match => match.Groups["placeholder"].Value switch
            {
                "{id}" => contentKey.ToString(),
                "{path}" => contentRoute.Path,
                "{start-id}" => contentRoute.StartItem.Id.ToString(),
                "{start-path}" => contentRoute.StartItem.Path,
                "{culture}" => culture
            })
            .Replace("//", "/")
            .TrimStart(Umbraco.Cms.Core.Constants.CharArrays.ForwardSlash);

        return $"{clientModel.Origin.TrimEnd(Umbraco.Cms.Core.Constants.CharArrays.ForwardSlash)}/{parsedUrlPath}".ToLowerInvariant();
    }

    [GeneratedRegex("(?<placeholder>{.[^}]*})", RegexOptions.IgnoreCase)]
    private static partial Regex PathPlaceholderParserRegex();
}
