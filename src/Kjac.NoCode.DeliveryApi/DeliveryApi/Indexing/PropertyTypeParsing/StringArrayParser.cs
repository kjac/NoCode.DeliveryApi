using Umbraco.Cms.Core.Serialization;

namespace Kjac.NoCode.DeliveryApi.DeliveryApi.Indexing.PropertyTypeParsing;

internal abstract class StringArrayParser : PropertyTypeParserBase
{
    private readonly IJsonSerializer _jsonSerializer;

    protected StringArrayParser(IJsonSerializer jsonSerializer)
        => _jsonSerializer = jsonSerializer;

    public override object[]? ParseIndexFieldValue(object propertyValue)
        => propertyValue is string stringValue
            ? ParseStringArrayValueAsFieldValue(stringValue, _jsonSerializer)
            : null;

}