using Kjac.NoCode.DeliveryApi.Models;
using Umbraco.Cms.Core;

namespace Kjac.NoCode.DeliveryApi.Services;

public interface IClientService
{
    Task<IEnumerable<ClientModel>> GetAllAsync();

    Task<Attempt<OperationStatus>> AddAsync(string name, string origin, string? previewUrlPath, string? publishedUrlPath, string? culture);

    Task<Attempt<OperationStatus>> UpdateAsync(Guid key, string name, string origin, string? previewUrlPath, string? publishedUrlPath, string? culture);

    Task<Attempt<OperationStatus>> DeleteAsync(Guid key);
}
