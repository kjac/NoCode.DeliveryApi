namespace Kjac.NoCode.DeliveryApi.ViewModels;

public sealed class UpdateClientRequestModel
{
    public required Guid Key { get; init; }

    public required string Name { get; init; }

    public required string Origin { get; init; }
}
