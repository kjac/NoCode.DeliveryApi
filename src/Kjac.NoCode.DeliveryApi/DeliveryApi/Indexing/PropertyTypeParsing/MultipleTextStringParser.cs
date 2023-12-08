using Umbraco.Cms.Core;

namespace Kjac.NoCode.DeliveryApi.DeliveryApi.Indexing.PropertyTypeParsing;

internal class MultipleTextStringParser : PropertyTypeParserBase
{
    public override object[]? ParseIndexFieldValue(object propertyValue)
        => propertyValue is string stringValue
            ? stringValue.Split(Umbraco.Cms.Core.Constants.CharArrays.LineFeed).OfType<object>().ToArray()
            : null;
}