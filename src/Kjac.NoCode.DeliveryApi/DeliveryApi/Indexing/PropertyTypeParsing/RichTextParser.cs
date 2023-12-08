using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;

namespace Kjac.NoCode.DeliveryApi.DeliveryApi.Indexing.PropertyTypeParsing;

internal class RichTextParser : PropertyTypeParserBase
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
        if (RichTextPropertyEditorHelper.TryParseRichTextEditorValue(propertyValue, _jsonSerializer, _logger, out var richTextEditorValue) is false)
        {
            return null;
        }
        
        // this is not perfect, but the RTE produces OK markup, so it will do for now.
        var valueWithoutTags = Regex.Replace(richTextEditorValue.Markup, "<[^>]*>", " ");
        return new object[] { Regex.Replace(valueWithoutTags, @"\s+", " ") };
    }
}