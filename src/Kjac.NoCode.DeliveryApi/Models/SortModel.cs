using Kjac.NoCode.DeliveryApi.Extensions;
using Umbraco.Cms.Core.DeliveryApi;

namespace Kjac.NoCode.DeliveryApi.Models;

public sealed class SortModel : QueryModelBase
{
    public string PropertyAlias { get; set; } = string.Empty;
    
    public override FieldType IndexFieldType => PrimitiveFieldType.SortFieldType();
}