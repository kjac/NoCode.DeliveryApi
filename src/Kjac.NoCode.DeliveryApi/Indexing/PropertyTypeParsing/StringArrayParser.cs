using Umbraco.Cms.Core.Serialization;

namespace Kjac.NoCode.DeliveryApi.Indexing.PropertyTypeParsing;

internal abstract class StringArrayParser : JsonPropertyTypeParserBase
{
    protected StringArrayParser(IJsonSerializer jsonSerializer)
        : base(jsonSerializer)
    {
    }

    public override object[]? ParseIndexFieldValue(object propertyValue)
        => propertyValue is string stringValue
            ? ParseStringArrayValueAsFieldValue(stringValue)
            : null;
}
