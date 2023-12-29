namespace Kjac.NoCode.DeliveryApi.ViewModels;

public sealed class UpdateFilterRequestModel
{
    public required Guid Key { get; init; }

    public required string Name { get; init; }

    public required string[] PropertyAliases { get; init; }
}
