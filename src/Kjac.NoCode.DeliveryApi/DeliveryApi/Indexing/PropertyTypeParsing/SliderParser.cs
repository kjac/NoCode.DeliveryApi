using Umbraco.Cms.Core;

namespace Kjac.NoCode.DeliveryApi.DeliveryApi.Indexing.PropertyTypeParsing;

internal class SliderParser : PropertyTypeParserBase
{
    public override object[]? ParseIndexFieldValue(object propertyValue)
        => propertyValue is string stringValue
            ? stringValue.Contains(',')
                ? stringValue
                    .Split(Umbraco.Cms.Core.Constants.CharArrays.Comma)
                    .Select(item => int.TryParse(item, out int itemValue) ? itemValue : (int?)null)
                    .Where(item => item is not null)
                    .OfType<object>()
                    .ToArray()
                : int.TryParse(stringValue, out int integerValue)
                    ? new object[] { integerValue }
                    : null
            : null;
}