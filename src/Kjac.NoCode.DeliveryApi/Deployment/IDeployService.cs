namespace Kjac.NoCode.DeliveryApi.Deployment;

internal interface IDeployService
{
    Task ExportAsync();

    Task ImportAsync();
}
