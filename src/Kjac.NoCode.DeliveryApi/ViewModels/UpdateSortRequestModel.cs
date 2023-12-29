namespace Kjac.NoCode.DeliveryApi.ViewModels;

public sealed class UpdateSortRequestModel
{
    public required Guid Key { get; init; }

    public required string Name { get; init; }

    public required string PropertyAlias { get; init; }
}
