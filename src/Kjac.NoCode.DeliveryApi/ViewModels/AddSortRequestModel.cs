using Kjac.NoCode.DeliveryApi.Models;

namespace Kjac.NoCode.DeliveryApi.ViewModels;

public sealed class AddSortRequestModel
{
    public required string Name { get; init; }

    public required string PropertyAlias { get; init; }

    public required PrimitiveFieldType PrimitiveFieldType { get; init; }
}