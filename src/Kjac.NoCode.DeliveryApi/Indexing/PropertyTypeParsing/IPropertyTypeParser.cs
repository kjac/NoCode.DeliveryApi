namespace Kjac.NoCode.DeliveryApi.Indexing.PropertyTypeParsing;

internal interface IPropertyTypeParser
{
    public object[]? ParseIndexFieldValue(object propertyValue);
}
