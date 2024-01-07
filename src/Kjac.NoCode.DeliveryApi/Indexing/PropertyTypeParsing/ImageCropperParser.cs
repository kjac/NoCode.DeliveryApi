using Umbraco.Cms.Core.Serialization;
using Umbraco.Extensions;

namespace Kjac.NoCode.DeliveryApi.Indexing.PropertyTypeParsing;

internal sealed class ImageCropperParser : PropertyTypeParserBase
{
    private readonly IJsonSerializer _jsonSerializer;

    public ImageCropperParser(IJsonSerializer jsonSerializer)
        => _jsonSerializer = jsonSerializer;

    public override object[]? ParseIndexFieldValue(object propertyValue)
    {
        if (propertyValue is not string imageCropperValue || imageCropperValue.DetectIsJson() is false)
        {
            return null;
        }

        ImageCropperDto? dto = _jsonSerializer.Deserialize<ImageCropperDto>(imageCropperValue);
        return dto is not null
            ? new object[] { dto.Src }
            : null;
    }

    private class ImageCropperDto
    {
        public required string Src { get; init; }
    }
}
