using Kjac.NoCode.DeliveryApi.Models;
using Umbraco.Cms.Core.DeliveryApi;

namespace Kjac.NoCode.DeliveryApi.Extensions;

internal static class PrimitiveFieldTypeExtensions
{
    public static FieldType FilterFieldType(this PrimitiveFieldType primitiveFieldType, FilterMatchType filterMatchType)
        => primitiveFieldType switch
        {
            PrimitiveFieldType.String => filterMatchType is FilterMatchType.Exact
                ? FieldType.StringRaw
                : FieldType.StringAnalyzed,
            PrimitiveFieldType.Number => FieldType.Number,
            PrimitiveFieldType.Date => FieldType.Date,
            _ => throw new ArgumentOutOfRangeException(nameof(primitiveFieldType))
        };

    public static FieldType SortFieldType(this PrimitiveFieldType primitiveFieldType)
        => primitiveFieldType switch
        {
            PrimitiveFieldType.String => FieldType.StringSortable,
            PrimitiveFieldType.Number => FieldType.Number,
            PrimitiveFieldType.Date => FieldType.Date,
            _ => throw new ArgumentOutOfRangeException(nameof(primitiveFieldType))
        };
}
