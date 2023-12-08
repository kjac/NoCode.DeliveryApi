using Kjac.NoCode.DeliveryApi.Models;

namespace Kjac.NoCode.DeliveryApi.ViewModels;

public sealed class AddFilterRequestModel
{
    public required string Name { get; init; }

    public required string[] PropertyAliases { get; init; }

    public required FilterMatchType FilterMatchType { get; init; }

    public required PrimitiveFieldType PrimitiveFieldType { get; init; }
}