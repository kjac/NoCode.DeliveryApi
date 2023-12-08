using Umbraco.Cms.Core.DeliveryApi;

namespace Kjac.NoCode.DeliveryApi.Models;

public abstract class ModelBase : IIndexFieldModel
{
    public Guid Key { get; init; }

    public string Name { get; set; } = string.Empty;

    public string Alias { get; set; } = string.Empty;

    public string IndexFieldName { get; init; } = string.Empty;

    public PrimitiveFieldType PrimitiveFieldType { get; init; }

    public abstract FieldType IndexFieldType { get; }
}