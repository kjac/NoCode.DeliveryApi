using Kjac.NoCode.DeliveryApi.Models;

namespace Kjac.NoCode.DeliveryApi.Services;

public interface IClientService
{
    Task<IEnumerable<ClientModel>> GetAllAsync();

    Task<bool> AddAsync(string name, string origin);

    Task<bool> UpdateAsync(Guid key, string name, string origin);

    Task<bool> DeleteAsync(Guid key);
}