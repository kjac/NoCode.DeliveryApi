using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Models.Dtos;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Scoping;

namespace Kjac.NoCode.DeliveryApi.Repositories;

internal class ClientRepository : RepositoryBase<ClientDto, ClientModel>, IClientRepository
{
    public ClientRepository(IScopeProvider scopeProvider, IRuntimeState runtimeState)
        : base(scopeProvider, runtimeState)
    {
    }

    protected override ClientModel ParseModel(ClientDto dto)
        => new ClientModel
        {
            Key = dto.Key,
            Name = dto.Name,
            Origin = dto.Origin
        };

    protected override ClientDto MapModelToDto(ClientModel clientModel, ClientDto dto)
    {
        dto.Key = clientModel.Key;
        dto.Name = clientModel.Name;
        dto.Origin = clientModel.Origin;

        return dto;
    }
}
