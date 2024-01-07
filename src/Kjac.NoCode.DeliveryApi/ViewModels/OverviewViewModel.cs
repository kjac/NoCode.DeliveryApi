using System.Runtime.Serialization;

namespace Kjac.NoCode.DeliveryApi.ViewModels;

[DataContract]
internal sealed class OverviewViewModel
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
