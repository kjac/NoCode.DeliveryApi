using System.Runtime.Serialization;
using Kjac.NoCode.DeliveryApi.Models;

namespace Kjac.NoCode.DeliveryApi.ViewModels;

internal class FilterViewModel : QueryViewModelBase
{
    [DataMember(Name = "propertyAliases")]
    public required IEnumerable<string> PropertyAliases { get; init; }

    [DataMember(Name = "filterMatchType")]
    public required FilterMatchType FilterMatchType { get; init; }
}