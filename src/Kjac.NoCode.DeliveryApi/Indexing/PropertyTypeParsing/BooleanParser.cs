namespace Kjac.NoCode.DeliveryApi.Indexing.PropertyTypeParsing;

internal class BooleanParser : PropertyTypeParserBase
{
    public override object[]? ParseIndexFieldValue(object propertyValue)
        => propertyValue is bool boolValue
            ? new object[] { boolValue }
            : propertyValue is int intValue
                ? new object[] { intValue == 1 }
                : propertyValue is string stringValue && bool.TryParse(stringValue, out bool parsedBoolValue)
                    ? new object[] { parsedBoolValue }
                    : null;
}
