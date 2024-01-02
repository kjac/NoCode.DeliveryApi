using System.Runtime.Serialization;

namespace Kjac.NoCode.DeliveryApi.ViewModels;

public class ClientViewModel : ViewModelBase
{
    [DataMember(Name = "origin")]
    public required string Origin { get; init; }

    [DataMember(Name = "previewUrlPath")]
    public string? PreviewUrlPath { get; init; }

    [DataMember(Name = "publishedUrlPath")]
    public string? PublishedUrlPath { get; init; }

    [DataMember(Name = "culture")]
    public string? Culture { get; init; }
}
