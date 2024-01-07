using Umbraco.Cms.Core.Serialization;
using Umbraco.Extensions;

namespace Kjac.NoCode.DeliveryApi.Indexing.PropertyTypeParsing;

internal sealed class TagsParser : JsonPropertyTypeParserBase

{
    public TagsParser(IJsonSerializer jsonSerializer)
        : base(jsonSerializer)
    {
    }

    public override object[]? ParseIndexFieldValue(object propertyValue)
    {
        if (propertyValue is not string tagsValue)
        {
            return null;
        }

        return tagsValue.DetectIsJson()
            ? ParseStringArrayValueAsFieldValue(tagsValue)
            : ParseCsvValue(tagsValue);
    }
}
