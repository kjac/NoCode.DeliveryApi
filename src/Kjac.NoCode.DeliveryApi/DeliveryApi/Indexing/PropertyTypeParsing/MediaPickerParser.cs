using Umbraco.Cms.Core.Serialization;
using Umbraco.Extensions;

namespace Kjac.NoCode.DeliveryApi.DeliveryApi.Indexing.PropertyTypeParsing;

internal class MediaPickerParser : PropertyTypeParserBase
{
    private readonly IJsonSerializer _jsonSerializer;

    public MediaPickerParser(IJsonSerializer jsonSerializer)
        => _jsonSerializer = jsonSerializer;

    public override object[]? ParseIndexFieldValue(object propertyValue)
    {
        if (propertyValue is not string mediaPickerValue || mediaPickerValue.DetectIsJson() is false)
        {
            return null;
        }

        var dtos = _jsonSerializer.Deserialize<MediaPickerDto[]>(mediaPickerValue);
        return dtos?.Select(dto => dto.MediaKey).OfType<object>().ToArray();
    }

    private class MediaPickerDto
    {
        public Guid MediaKey { get; set; }
    }
}