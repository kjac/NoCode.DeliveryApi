namespace Kjac.NoCode.DeliveryApi.ViewModels;

internal sealed class OverviewViewModel
{
    public required IEnumerable<FilterViewModel> Filters { get; init; }

    public required IEnumerable<SortViewModel> Sorts { get; init; }

    public bool CanAddFilter { get; set; }

    public bool CanAddSort { get; set; }
}
