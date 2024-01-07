namespace Kjac.NoCode.DeliveryApi.Indexing.PropertyTypeParsing;

internal sealed class MultipleTextStringParser : PropertyTypeParserBase
{
    public override object[]? ParseIndexFieldValue(object propertyValue)
        => propertyValue is string stringValue
            ? stringValue.Split(Umbraco.Cms.Core.Constants.CharArrays.LineFeed).OfType<object>().ToArray()
            : null;
}
