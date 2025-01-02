namespace Kjac.NoCode.DeliveryApi.ViewModels;

internal sealed class ClientViewModel : ViewModelBase
{
    public required string Origin { get; init; }

    public string? PreviewUrlPath { get; init; }

    public string? PublishedUrlPath { get; init; }

    public string? Culture { get; init; }
}
