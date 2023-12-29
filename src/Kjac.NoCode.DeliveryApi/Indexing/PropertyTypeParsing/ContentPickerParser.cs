namespace Kjac.NoCode.DeliveryApi.Indexing.PropertyTypeParsing;

internal class ContentPickerParser : PropertyTypeParserBase
{
    public override object[]? ParseIndexFieldValue(object propertyValue)
    {
        if (propertyValue is not string udiValue)
        {
            return null;
        }

        Guid? guid = ParseUdiValue(udiValue);
        return guid is not null
            ? new object[] { guid }
            : null;
    }
}
