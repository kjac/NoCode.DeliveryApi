namespace Kjac.NoCode.DeliveryApi.ViewModels;

internal abstract class ViewModelBase
{
    public required Guid Id { get; init; }

    public required string Name { get; init; }
}
