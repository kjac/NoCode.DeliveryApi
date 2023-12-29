using System.Runtime.Serialization;

namespace Kjac.NoCode.DeliveryApi.ViewModels;

public class ClientViewModel : ViewModelBase
{
    [DataMember(Name = "origin")]
    public required string Origin { get; init; }
}
