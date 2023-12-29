using Kjac.NoCode.DeliveryApi.Services;
using Kjac.NoCode.DeliveryApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;

namespace Kjac.NoCode.DeliveryApi.Controllers;

[PluginController(Constants.BackOfficeArea)]
public sealed class ClientConfigurationController : UmbracoAuthorizedJsonController
{
    private readonly IClientService _clientService;
    private readonly ICorsPolicyService _corsPolicyService;

    public ClientConfigurationController(IClientService clientService, ICorsPolicyService corsPolicyService)
    {
        _clientService = clientService;
        _corsPolicyService = corsPolicyService;
    }

    [HttpGet]
    public async Task<IActionResult> All()
    {
        var clients = await _clientService.GetAllAsync();
        return Ok(clients.Select(client => new ClientViewModel
        {
            Key = client.Key,
            Name = client.Name,
            Origin = client.Origin
        }));
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddClientRequestModel requestModel)
    {
        var result = await _clientService.AddAsync(requestModel.Name, requestModel.Origin);
        return await ApplyCorsPoliciesOnSuccess(result);
    }
    
    [HttpPut]
    public async Task<IActionResult> Update(UpdateClientRequestModel requestModel)
    {
        var result = await _clientService.UpdateAsync(requestModel.Key, requestModel.Name, requestModel.Origin);
        return await ApplyCorsPoliciesOnSuccess(result);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid key)
    {
        var result = await _clientService.DeleteAsync(key);
        return await ApplyCorsPoliciesOnSuccess(result);
    }

    // NOTE: this must be moved to an event handler eventually (to support load balancing), so we'll just handle it quick and dirty for now
    private async Task<IActionResult> ApplyCorsPoliciesOnSuccess(bool result)
    {
        if (result)
        {
            await _corsPolicyService.ApplyClientOriginsAsync();
        }

        return result ? Ok() : BadRequest();
    }
}