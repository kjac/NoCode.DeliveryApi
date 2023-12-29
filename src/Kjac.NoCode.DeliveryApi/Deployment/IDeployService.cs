namespace Kjac.NoCode.DeliveryApi.Deployment;

public interface IDeployService
{
    Task ExportAsync();

    Task ImportAsync();
}