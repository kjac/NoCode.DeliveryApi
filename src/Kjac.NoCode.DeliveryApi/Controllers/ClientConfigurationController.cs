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
        => await ApplyCorsPoliciesOnChange(async () => await _clientService.AddAsync(
            requestModel.Name,
            requestModel.Origin));

    [HttpPut]
    public async Task<IActionResult> Update(UpdateClientRequestModel requestModel)
        => await ApplyCorsPoliciesOnChange(async () => await _clientService.UpdateAsync(
            requestModel.Key,
            requestModel.Name,
            requestModel.Origin));

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid key)
        => await ApplyCorsPoliciesOnChange(async () => await _clientService.DeleteAsync(key));

    // NOTE: this must be moved to an event handler eventually (to support load balancing), so we'll just handle it quick and dirty for now
    private async Task<IActionResult> ApplyCorsPoliciesOnChange(Func<Task<bool>> action)
    {
        var result = await action();
        if (result)
        {
            await _corsPolicyService.ApplyClientOriginsAsync();
        }

        return result ? Ok() : BadRequest();
    }
}