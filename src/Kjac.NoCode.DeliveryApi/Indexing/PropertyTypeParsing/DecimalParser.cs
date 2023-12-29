using System.Globalization;

namespace Kjac.NoCode.DeliveryApi.Indexing.PropertyTypeParsing;

internal class DecimalParser : PropertyTypeParserBase
{
    public override object[]? ParseIndexFieldValue(object propertyValue)
        => propertyValue is decimal decimalValue
            ? new object[] { decimalValue.ToString("N2", CultureInfo.InvariantCulture) }
            : propertyValue is string stringValue
                ? new object[] { stringValue }
                : null;
}
