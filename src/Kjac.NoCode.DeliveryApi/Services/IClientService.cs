using Kjac.NoCode.DeliveryApi.Models;

namespace Kjac.NoCode.DeliveryApi.Services;

public interface IClientService
{
    Task<IEnumerable<ClientModel>> GetAllAsync();

    Task<bool> AddAsync(string name, string origin, string? previewUrlPath, string? publishedUrlPath, string? culture);

    Task<bool> UpdateAsync(Guid key, string name, string origin, string? previewUrlPath, string? publishedUrlPath, string? culture);

    Task<bool> DeleteAsync(Guid key);
}
