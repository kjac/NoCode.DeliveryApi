using Umbraco.Cms.Core;
using Umbraco.Extensions;

namespace Kjac.NoCode.DeliveryApi.DeliveryApi.Indexing.PropertyTypeParsing;

internal abstract class PropertyTypeParserBase : IPropertyTypeParser
{
    public abstract object[]? ParseIndexFieldValue(object propertyValue);

    protected Guid? ParseUdiValue(string udiValue)
        => UdiParser.TryParse(udiValue, out Udi? udi) && udi is GuidUdi guidUdi
            ? guidUdi.Guid
            : null;

    protected object[] ParseCsvValue(string csvValue)
        => csvValue
            .Split(Umbraco.Cms.Core.Constants.CharArrays.Comma, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .WhereNotNull()
            .OfType<object>()
            .ToArray();
}