using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Models.Dtos;
using Umbraco.Cms.Core.Services;
using Umbraco.Cms.Infrastructure.Scoping;
using Umbraco.Extensions;

namespace Kjac.NoCode.DeliveryApi.Repositories;

internal sealed class ClientRepository : RepositoryBase<ClientDto, ClientModel>, IClientRepository
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
            Origin = dto.Origin,
            PreviewUrlPath = dto.PreviewUrlPath,
            PublishedUrlPath = dto.PublishedUrlPath,
            Culture = dto.Culture
        };

    protected override ClientDto MapModelToDto(ClientModel clientModel, ClientDto dto)
    {
        dto.Key = clientModel.Key;
        dto.Name = clientModel.Name;
        dto.Origin = clientModel.Origin;
        dto.PreviewUrlPath = clientModel.PreviewUrlPath.NullOrWhiteSpaceAsNull();
        dto.PublishedUrlPath = clientModel.PublishedUrlPath.NullOrWhiteSpaceAsNull();
        dto.Culture = clientModel.Culture.NullOrWhiteSpaceAsNull();

        return dto;
    }
}
