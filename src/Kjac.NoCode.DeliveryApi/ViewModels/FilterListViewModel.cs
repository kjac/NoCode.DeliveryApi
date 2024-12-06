namespace Kjac.NoCode.DeliveryApi.ViewModels;

internal sealed class FilterListViewModel
{
    public required IEnumerable<FilterViewModel> Filters { get; init; }

    public bool CanAddFilter { get; set; }
}
