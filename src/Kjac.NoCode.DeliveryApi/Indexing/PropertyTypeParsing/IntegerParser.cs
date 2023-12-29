namespace Kjac.NoCode.DeliveryApi.Indexing.PropertyTypeParsing;

internal class IntegerParser : PropertyTypeParserBase
{
    public override object[]? ParseIndexFieldValue(object propertyValue)
        => propertyValue is int intValue
            ? new object[] { intValue }
            : propertyValue is string stringValue && int.TryParse(stringValue, out int parsedIntValue)
                ? new object[] { parsedIntValue }
                : null;
}
