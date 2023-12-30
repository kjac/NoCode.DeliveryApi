using Kjac.NoCode.DeliveryApi.Repositories;
using Kjac.NoCode.DeliveryApi.Services;
using Umbraco.Cms.Core.Cache;

namespace Kjac.NoCode.DeliveryApi.Caching;

internal sealed class ClientsCacheRefresher : ICacheRefresher
{
    private readonly IClientRepository _clientRepository;
    private readonly ICorsPolicyService _corsPolicyService;

    public static readonly Guid UniqueId = Guid.Parse("64890767-8ADC-4873-A97D-55DB8F530C09");

    public ClientsCacheRefresher(IClientRepository clientRepository, ICorsPolicyService corsPolicyService)
    {
        _clientRepository = clientRepository;
        _corsPolicyService = corsPolicyService;
    }

    public void RefreshAll()
    {
        _clientRepository.ReloadCache();
        _corsPolicyService.ApplyClientOriginsAsync().GetAwaiter().GetResult();
    }

    public void Refresh(int id) => throw new NotImplementedException();

    public void Remove(int id) => throw new NotImplementedException();

    public void Refresh(Guid id) => throw new NotImplementedException();

    public Guid RefresherUniqueId => UniqueId;

    public string Name => "No-Code Delivery API - Clients Cache Refresher";
}
