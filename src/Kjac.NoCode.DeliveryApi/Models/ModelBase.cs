namespace Kjac.NoCode.DeliveryApi.Models;

public abstract class ModelBase
{
    public Guid Key { get; init; }

    public string Name { get; set; } = string.Empty;
}
