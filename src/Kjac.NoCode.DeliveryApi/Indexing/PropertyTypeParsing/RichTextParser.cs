using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;

namespace Kjac.NoCode.DeliveryApi.Indexing.PropertyTypeParsing;

internal partial class RichTextParser : PropertyTypeParserBase
{
    private readonly IJsonSerializer _jsonSerializer;
    private readonly ILogger _logger;

    public RichTextParser(IJsonSerializer jsonSerializer, ILogger logger)
    {
        _jsonSerializer = jsonSerializer;
        _logger = logger;
    }

    public override object[]? ParseIndexFieldValue(object propertyValue)
    {
        if (RichTextPropertyEditorHelper.TryParseRichTextEditorValue(
                propertyValue,
                _jsonSerializer,
                _logger,
                out RichTextEditorValue? richTextEditorValue) is false)
        {
            return null;
        }

        // this is not perfect, but the RTE produces OK markup, so it will do for now.
        var valueWithoutTags = TagsRegex().Replace(richTextEditorValue.Markup, " ");
        return new object[] { WhitespaceCleanupRegex().Replace(valueWithoutTags, " ") };
    }

    [GeneratedRegex("<[^>]*>")]
    private static partial Regex TagsRegex();

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceCleanupRegex();
}
