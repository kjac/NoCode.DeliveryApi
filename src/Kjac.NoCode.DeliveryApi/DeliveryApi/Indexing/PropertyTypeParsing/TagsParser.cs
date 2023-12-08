using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Extensions;

namespace Kjac.NoCode.DeliveryApi.DeliveryApi.Indexing.PropertyTypeParsing;

internal class TagsParser : PropertyTypeParserBase
{
    private readonly IJsonSerializer _jsonSerializer;

    public TagsParser(IJsonSerializer jsonSerializer)
        => _jsonSerializer = jsonSerializer;

    public override object[]? ParseIndexFieldValue(object propertyValue)
    {
        if (propertyValue is not string tagsValue)
        {
            return null;
        }

        if (tagsValue.DetectIsJson() is false)
        {
            // CSV format
            return tagsValue.Split(Umbraco.Cms.Core.Constants.CharArrays.Comma).OfType<object>().ToArray();
        }

        return ParseStringArrayValueAsFieldValue(tagsValue, _jsonSerializer);
    }
}