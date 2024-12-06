namespace Kjac.NoCode.DeliveryApi.ViewModels;

internal sealed class SortListViewModel
{
    public required IEnumerable<SortViewModel> Sorts { get; init; }

    public bool CanAddSort { get; set; }
}
