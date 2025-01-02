using Kjac.NoCode.DeliveryApi.Models;

namespace Kjac.NoCode.DeliveryApi.ViewModels;

internal abstract class QueryViewModelBase : ViewModelBase
{
    public required string Alias { get; init; }

    public required string FieldName { get; init; }

    public required PrimitiveFieldType PrimitiveFieldType { get; init; }
}
