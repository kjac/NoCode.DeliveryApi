using Kjac.NoCode.DeliveryApi.Extensions;
using Umbraco.Cms.Core.DeliveryApi;

namespace Kjac.NoCode.DeliveryApi.Models;

public sealed class FilterModel : QueryModelBase
{
    public IEnumerable<string> PropertyAliases { get; set; } = Enumerable.Empty<string>();

    public FilterMatchType FilterMatchType { get; set; }

    public override FieldType IndexFieldType => PrimitiveFieldType.FilterFieldType(FilterMatchType);
}