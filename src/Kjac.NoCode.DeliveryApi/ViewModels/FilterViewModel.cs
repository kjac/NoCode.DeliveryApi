using Kjac.NoCode.DeliveryApi.Models;

namespace Kjac.NoCode.DeliveryApi.ViewModels;

internal sealed class FilterViewModel : QueryViewModelBase
{
    public required IEnumerable<string> PropertyAliases { get; init; }

    public required FilterMatchType FilterMatchType { get; init; }
}
