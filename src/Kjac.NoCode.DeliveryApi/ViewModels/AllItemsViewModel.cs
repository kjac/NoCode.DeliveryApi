using System.Runtime.Serialization;

namespace Kjac.NoCode.DeliveryApi.ViewModels;

[DataContract]
internal class AllItemsViewModel
{
    [DataMember(Name = "filters")]
    public required IEnumerable<FilterViewModel> Filters { get; init; }
    
    [DataMember(Name = "sorts")]
    public required IEnumerable<SortViewModel> Sorts { get; init; }

    [DataMember(Name = "canAddFilter")]
    public bool CanAddFilter { get; set; }

    [DataMember(Name = "canAddSort")]
    public bool CanAddSort { get; set; }
}