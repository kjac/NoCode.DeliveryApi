using Kjac.NoCode.DeliveryApi.Models;
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

    public ClientConfigurationController(IClientService clientService)
        => _clientService = clientService;

    [HttpGet]
    public async Task<IActionResult> All()
    {
        IEnumerable<ClientModel> clients = await _clientService.GetAllAsync();
        return Ok(clients.Select(client => new ClientViewModel
        {
            Key = client.Key,
            Name = client.Name,
            Origin = client.Origin,
            PreviewUrlPath = client.PreviewUrlPath,
            PublishedUrlPath = client.PublishedUrlPath,
            Culture = client.Culture
        }));
    }

    [HttpPost]
    public async Task<IActionResult> Add(AddClientRequestModel requestModel)
        => await _clientService.AddAsync(
            requestModel.Name,
            requestModel.Origin,
            requestModel.PreviewUrlPath,
            requestModel.PublishedUrlPath,
            requestModel.Culture)
        ? Ok()
        : BadRequest();

    [HttpPut]
    public async Task<IActionResult> Update(UpdateClientRequestModel requestModel)
        => await _clientService.UpdateAsync(
            requestModel.Key,
            requestModel.Name,
            requestModel.Origin,
            requestModel.PreviewUrlPath,
            requestModel.PublishedUrlPath,
            requestModel.Culture)
        ? Ok()
        : BadRequest();

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid key)
        => await _clientService.DeleteAsync(key)
        ? Ok()
        : BadRequest();
}
