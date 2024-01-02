namespace Kjac.NoCode.DeliveryApi.Models;

public sealed class ClientModel : ModelBase
{
    public string Origin { get; set; } = string.Empty;

    public string? PreviewUrlPath { get; set; }

    public string? PublishedUrlPath { get; set; }

    public string? Culture { get; set; }
}
