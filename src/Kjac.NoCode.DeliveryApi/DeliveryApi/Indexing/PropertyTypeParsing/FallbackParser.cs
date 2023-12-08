namespace Kjac.NoCode.DeliveryApi.DeliveryApi.Indexing.PropertyTypeParsing;

internal class FallbackParser : PropertyTypeParserBase
{
    public override object[]? ParseIndexFieldValue(object propertyValue)
        => propertyValue switch
        {
            // fallback values go here
            string stringValue => new object[] { stringValue },
            IEnumerable<string> stringValues => stringValues.OfType<object>().ToArray(),
            int intValue => new object[] { intValue },
            IEnumerable<int> intValues => intValues.OfType<object>().ToArray(),
            DateTime dateTime => new object[] { dateTime },
            _ => null
        };
}