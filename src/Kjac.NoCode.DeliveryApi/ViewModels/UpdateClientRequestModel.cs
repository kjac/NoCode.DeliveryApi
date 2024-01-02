namespace Kjac.NoCode.DeliveryApi.ViewModels;

public sealed class UpdateClientRequestModel
{
    public required Guid Key { get; init; }

    public required string Name { get; init; }

    public required string Origin { get; init; }

    public string? PreviewUrlPath { get; init; }

    public string? PublishedUrlPath { get; init; }

    public string? Culture { get; init; }
}
