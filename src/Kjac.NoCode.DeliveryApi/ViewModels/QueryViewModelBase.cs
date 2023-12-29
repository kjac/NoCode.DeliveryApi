using System.Runtime.Serialization;
using Kjac.NoCode.DeliveryApi.Models;

namespace Kjac.NoCode.DeliveryApi.ViewModels;

public abstract class QueryViewModelBase : ViewModelBase
{
    [DataMember(Name = "alias")]
    public required string Alias { get; init; }

    [DataMember(Name = "fieldName")]
    public required string FieldName { get; init; }

    [DataMember(Name = "primitiveFieldType")]
    public required PrimitiveFieldType PrimitiveFieldType { get; init; }
}
