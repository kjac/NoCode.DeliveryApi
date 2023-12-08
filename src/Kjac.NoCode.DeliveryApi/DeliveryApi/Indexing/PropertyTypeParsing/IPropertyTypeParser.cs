namespace Kjac.NoCode.DeliveryApi.DeliveryApi.Indexing.PropertyTypeParsing;

internal interface IPropertyTypeParser
{
    public object[]? ParseIndexFieldValue(object propertyValue);
}