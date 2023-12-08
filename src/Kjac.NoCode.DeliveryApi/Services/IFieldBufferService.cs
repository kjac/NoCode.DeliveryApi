using Kjac.NoCode.DeliveryApi.Models;
using Umbraco.Cms.Core.DeliveryApi;

namespace Kjac.NoCode.DeliveryApi.Services;

public interface IFieldBufferService
{
    IIndexFieldModel? GetField(FieldType fieldType);

    IEnumerable<IIndexFieldModel> AvailableFields();

    bool IsDepleted();
}