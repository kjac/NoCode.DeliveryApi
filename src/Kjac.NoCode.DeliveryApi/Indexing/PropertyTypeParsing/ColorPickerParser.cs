using Umbraco.Cms.Core.Serialization;
using Umbraco.Extensions;

namespace Kjac.NoCode.DeliveryApi.Indexing.PropertyTypeParsing;

internal sealed class ColorPickerParser : PropertyTypeParserBase
{
    private readonly IJsonSerializer _jsonSerializer;

    public ColorPickerParser(IJsonSerializer jsonSerializer)
        => _jsonSerializer = jsonSerializer;

    public override object[]? ParseIndexFieldValue(object propertyValue)
    {
        if (propertyValue is not string colorPickerValue || colorPickerValue.DetectIsJson() is false)
        {
            return null;
        }

        ColorPickerDto? dto = _jsonSerializer.Deserialize<ColorPickerDto>(colorPickerValue);
        return dto is not null
            ? new object[] { dto.Value }
            : null;
    }

    private class ColorPickerDto
    {
        public required string Value { get; init; }
    }
}
