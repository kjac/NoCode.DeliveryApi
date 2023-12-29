using Kjac.NoCode.DeliveryApi.Services;
using Umbraco.Cms.Core.DeliveryApi;
using Umbraco.Cms.Core.Models;

namespace Kjac.NoCode.DeliveryApi.Indexing;

public sealed class LevelIndexer : IContentIndexHandler
{
    internal static readonly string FieldName = IndexFieldName.Get("Level");

    public IEnumerable<IndexFieldValue> GetFieldValues(IContent content, string? culture)
        => new[]
        {
            new IndexFieldValue
            {
                Values = new object[] { content.Level },
                FieldName = FieldName
            }
        };

    public IEnumerable<IndexField> GetFields()
        => new[]
        {
            new IndexField
            {
                FieldName = FieldName,
                FieldType = FieldType.Number,
                VariesByCulture = false
            }
        };
}
