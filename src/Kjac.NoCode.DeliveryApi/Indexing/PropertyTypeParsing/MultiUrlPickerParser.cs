using Umbraco.Cms.Core.Serialization;
using Umbraco.Extensions;

namespace Kjac.NoCode.DeliveryApi.Indexing.PropertyTypeParsing;

internal sealed class MultiUrlPickerParser : PropertyTypeParserBase
{
    private readonly IJsonSerializer _jsonSerializer;

    public MultiUrlPickerParser(IJsonSerializer jsonSerializer)
        => _jsonSerializer = jsonSerializer;

    public override object[]? ParseIndexFieldValue(object propertyValue)
    {
        if (propertyValue is not string multiUrlPickerValue || multiUrlPickerValue.DetectIsJson() is false)
        {
            return null;
        }

        MultiUrlPickerDto[]? dtos = _jsonSerializer.Deserialize<MultiUrlPickerDto[]>(multiUrlPickerValue);
        return dtos?.Select(dto =>
                dto.Udi is not null
                    // NOTE: returning a string value here because dto.Url will also yield a string.
                    //       it hardly matters, though, as GUIDs will be treated as strings down the line.
                    ? ParseUdiValue(dto.Udi)?.ToString()
                    : dto.Url is not null
                        ? $"{dto.Url}{dto.QueryString}"
                        : null
            )
            .WhereNotNull()
            .OfType<object>()
            .ToArray();
    }

    private class MultiUrlPickerDto
    {
        public string? Udi { get; init; }

        public string? Url { get; init; }

        public string? QueryString { get; init; }
    }
}
