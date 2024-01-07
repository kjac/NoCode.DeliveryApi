namespace Kjac.NoCode.DeliveryApi.Indexing.PropertyTypeParsing;

internal sealed class MultiNodeTreePickerParser : PropertyTypeParserBase
{
    public override object[]? ParseIndexFieldValue(object propertyValue)
        => propertyValue is string stringValue
            ? stringValue
                .Split(Umbraco.Cms.Core.Constants.CharArrays.Comma).Select(ParseUdiValue)
                .Where(guid => guid.HasValue)
                .OfType<object>()
                .ToArray()
            : null;
}
