namespace Kjac.NoCode.DeliveryApi.Services;

public enum OperationStatus
{
    Success,
    NotFound,
    UnknownIndexFieldName,
    DuplicateAlias,
    FailedCreate,
    FailedUpdate,
    FailedDelete
}
