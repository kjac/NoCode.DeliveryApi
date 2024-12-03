namespace Kjac.NoCode.DeliveryApi.ViewModels;

public sealed class UpdateFilterRequestModel
{
    public required string Name { get; init; }

    public required string[] PropertyAliases { get; init; }
}
