using Umbraco.Cms.Core.Serialization;
using Umbraco.Extensions;

namespace Kjac.NoCode.DeliveryApi.Indexing.PropertyTypeParsing;

internal abstract class JsonPropertyTypeParserBase : PropertyTypeParserBase
{
    private readonly IJsonSerializer _jsonSerializer;

    protected JsonPropertyTypeParserBase(IJsonSerializer jsonSerializer)
        => _jsonSerializer = jsonSerializer;

    protected object[]? ParseStringArrayValueAsFieldValue(string stringArrayValue)
        => _jsonSerializer.Deserialize<string[]>(stringArrayValue)?.WhereNotNull().OfType<object>().ToArray();
}
