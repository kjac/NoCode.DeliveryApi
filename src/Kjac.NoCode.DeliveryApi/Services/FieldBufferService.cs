using Kjac.NoCode.DeliveryApi.Models;
using Umbraco.Cms.Core.DeliveryApi;

namespace Kjac.NoCode.DeliveryApi.Services;

internal sealed class FieldBufferService : IFieldBufferService
{
    private readonly FieldType[] _allFieldTypes = new[]
    {
        FieldType.StringRaw, FieldType.StringAnalyzed, FieldType.StringSortable, FieldType.Number, FieldType.Date
    };

    private readonly List<BufferIndexFieldModel> _buffer;

    public FieldBufferService()
        => _buffer = _allFieldTypes.SelectMany(fieldType => CreateFields(fieldType, 20)).ToList();

    public IIndexFieldModel? GetField(FieldType fieldType)
    {
        BufferIndexFieldModel? next = _buffer.FirstOrDefault(f => f.IndexFieldType == fieldType);
        if (next is null)
        {
            return null;
        }

        _buffer.Remove(next);
        return next;
    }

    public IEnumerable<IIndexFieldModel> AvailableFields() => _buffer;

    public bool IsDepleted() => _allFieldTypes.Any(IsDepleted);

    private static BufferIndexFieldModel[] CreateFields(FieldType fieldType, int count)
        => Enumerable.Range(0, count).Select(_ => CreateField(fieldType)).ToArray();

    private bool IsDepleted(FieldType fieldType)
        => _buffer.Any(f => f.IndexFieldType == fieldType) is false;

    private static BufferIndexFieldModel CreateField(FieldType fieldType)
        => new()
        {
            IndexFieldName = IndexFieldName.GetRandom(),
            IndexFieldType = fieldType
        };
}
