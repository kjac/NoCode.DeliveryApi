using Asp.Versioning;
using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Services;
using Kjac.NoCode.DeliveryApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core;

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
    {
        Attempt<OperationStatus> result = await _clientService.AddAsync(
            requestModel.Name,
            requestModel.Origin,
            requestModel.PreviewUrlPath,
            requestModel.PublishedUrlPath,
            requestModel.Culture);

        return OperationStatusResult(result.Result);
    }

    [HttpPut("client/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, UpdateClientRequestModel requestModel)
    {
        Attempt<OperationStatus> result = await _clientService.UpdateAsync(
            id,
            requestModel.Name,
            requestModel.Origin,
            requestModel.PreviewUrlPath,
            requestModel.PublishedUrlPath,
            requestModel.Culture);

        return OperationStatusResult(result.Result);
    }

    [HttpDelete("client/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id)
    {
        Attempt<OperationStatus> result = await _clientService.DeleteAsync(id);

        return OperationStatusResult(result.Result);
    }
}
