using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Repositories;

namespace Kjac.NoCode.DeliveryApi.Services;

internal class ClientService : IClientService
{
    private readonly IClientRepository _repository;

    public ClientService(IClientRepository repository)
        => _repository = repository;

    public async Task<IEnumerable<ClientModel>> GetAllAsync()
        => await _repository.GetAllAsync();

    public async Task<bool> AddAsync(string name, string origin)
        => await _repository.CreateAsync(new ClientModel
        {
            Key = Guid.NewGuid(),
            Name = name,
            Origin = origin
        });

    public async Task<bool> UpdateAsync(Guid key, string name, string origin)
    {
        var model = await _repository.GetAsync(key);
        if (model is null)
        {
            return false;
        }

        model.Name = name;
        model.Origin = origin;

        return await _repository.UpdateAsync(model);
    }

    public async Task<bool> DeleteAsync(Guid key)
        => await _repository.DeleteAsync(key);
}