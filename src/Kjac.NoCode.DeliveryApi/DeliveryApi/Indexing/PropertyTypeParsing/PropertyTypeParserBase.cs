using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Serialization;

namespace Kjac.NoCode.DeliveryApi.DeliveryApi.Indexing.PropertyTypeParsing;

internal abstract class PropertyTypeParserBase : IPropertyTypeParser
{
    public abstract object[]? ParseIndexFieldValue(object propertyValue);

    protected Guid? ParseUdiValue(string udiValue)
        => UdiParser.TryParse(udiValue, out Udi? udi) && udi is GuidUdi guidUdi
            ? guidUdi.Guid
            : null;

    protected object[]? ParseStringArrayValueAsFieldValue(string stringArrayValue, IJsonSerializer jsonSerializer)
        => ParseArrayAsFieldValue(stringArrayValue, jsonSerializer, item => item.ToString());
    // => jsonSerializer.Deserialize<string[]>(stringArrayValue)?.OfType<object>().ToArray();

    // protected object[]? ParseIntegerArrayValueAsFieldValue(string integerArrayValue, IJsonSerializer jsonSerializer)
    //     => jsonSerializer.Deserialize<int[]>(stringArrayValue)?.OfType<object>().ToArray();

    protected object[]? ParseIntegerArrayValueAsFieldValue(string integerArrayValue, IJsonSerializer jsonSerializer)
        => ParseArrayAsFieldValue(integerArrayValue, jsonSerializer, item => int.TryParse(item.ToString(), out var value) ? value : (int?)null);

    private object[]? ParseArrayAsFieldValue<T>(string arrayValue, IJsonSerializer jsonSerializer, Func<object, T?> tryParseValue)
        => jsonSerializer
            .Deserialize<object[]>(arrayValue)?
            .Select(tryParseValue)
            .Where(value => value is not null)
            .OfType<object>()
            .ToArray();
}