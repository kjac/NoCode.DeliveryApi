namespace Kjac.NoCode.DeliveryApi.DeliveryApi.Indexing.PropertyTypeParsing;

internal class ContentPickerParser : PropertyTypeParserBase
{
    public override object[]? ParseIndexFieldValue(object propertyValue)
    {
        if (propertyValue is not string udiValue)
        {
            return null;
        }

        var guid = ParseUdiValue(udiValue);
        return guid is not null
            ? new object[] { guid }
            : null;
    }
}