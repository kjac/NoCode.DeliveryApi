using System.Runtime.Serialization;

namespace Kjac.NoCode.DeliveryApi.ViewModels;

[DataContract]
internal class SortViewModel : ItemViewModelBase
{
    [DataMember(Name = "propertyAlias")]
    public required string PropertyAlias { get; init; }
}