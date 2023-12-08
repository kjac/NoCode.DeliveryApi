using Umbraco.Cms.Core.DeliveryApi;

namespace Kjac.NoCode.DeliveryApi.Models;

public interface IIndexFieldModel
{
    public string IndexFieldName { get; }

    public FieldType IndexFieldType { get; }
}