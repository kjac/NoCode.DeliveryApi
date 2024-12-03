using Asp.Versioning;
using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Services;
using Kjac.NoCode.DeliveryApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kjac.NoCode.DeliveryApi.Controllers;

[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Clients")]
public sealed class ClientConfigurationController : NoCodeDeliveryApiControllerBase
{
    private readonly IClientService _clientService;

    public ClientConfigurationController(IClientService clientService)
        => _clientService = clientService;

    [HttpGet("client")]
    [ProducesResponseType<IEnumerable<ClientViewModel>>(StatusCodes.Status200OK)]
    public async Task<IActionResult> All()
    {
        IEnumerable<ClientModel> clients = await _clientService.GetAllAsync();
        return Ok(clients.Select(client => new ClientViewModel
        {
            Id = client.Key,
            Name = client.Name,
            Origin = client.Origin,
            PreviewUrlPath = client.PreviewUrlPath,
            PublishedUrlPath = client.PublishedUrlPath,
            Culture = client.Culture
        }));
    }

    [HttpPost("client")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add(AddClientRequestModel requestModel)
        => await _clientService.AddAsync(
            requestModel.Name,
            requestModel.Origin,
            requestModel.PreviewUrlPath,
            requestModel.PublishedUrlPath,
            requestModel.Culture)
        ? Ok()
        : BadRequest();

    [HttpPut("client/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, UpdateClientRequestModel requestModel)
        => await _clientService.UpdateAsync(
            id,
            requestModel.Name,
            requestModel.Origin,
            requestModel.PreviewUrlPath,
            requestModel.PublishedUrlPath,
            requestModel.Culture)
        ? Ok()
        : BadRequest();

    [HttpDelete("client/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id)
        => await _clientService.DeleteAsync(id)
        ? Ok()
        : BadRequest();
}
