using System.Runtime.Serialization;

namespace Kjac.NoCode.DeliveryApi.ViewModels;

internal class SortViewModel : QueryViewModelBase
{
    [DataMember(Name = "propertyAlias")]
    public required string PropertyAlias { get; init; }
}
