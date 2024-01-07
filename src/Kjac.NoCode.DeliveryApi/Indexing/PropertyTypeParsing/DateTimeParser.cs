using System.Globalization;

namespace Kjac.NoCode.DeliveryApi.Indexing.PropertyTypeParsing;

internal sealed class DateTimeParser : PropertyTypeParserBase
{
    public override object[]? ParseIndexFieldValue(object propertyValue)
        => propertyValue is DateTime dateTimeValue
            ? new object[] { dateTimeValue }
            : propertyValue is string stringValue && DateTime.TryParse(stringValue, CultureInfo.InvariantCulture,
                out DateTime parsedDateTime)
                ? new object[] { parsedDateTime }
                : null;
}
