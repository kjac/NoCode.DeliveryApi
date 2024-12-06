using Asp.Versioning;
using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Services;
using Kjac.NoCode.DeliveryApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kjac.NoCode.DeliveryApi.Controllers;

[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Filters")]
public class FilterConfigurationController : NoCodeDeliveryApiControllerBase
{
    private readonly IFilterService _filterService;

    public FilterConfigurationController(IFilterService filterService)
        => _filterService = filterService;

    [HttpGet("filter")]
    [ProducesResponseType<FilterListViewModel>(StatusCodes.Status200OK)]
    public async Task<IActionResult> All()
    {
        IEnumerable<FilterModel> filters = await _filterService.GetAllAsync();
        var canAddFilter = _filterService.CanAdd();
        return Ok(new FilterListViewModel
        {
            Filters = filters.Select(filter => new FilterViewModel
            {
                Id = filter.Key,
                Name = filter.Name,
                Alias = filter.Alias,
                FieldName = filter.IndexFieldName,
                PrimitiveFieldType = filter.PrimitiveFieldType,
                PropertyAliases = filter.PropertyAliases,
                FilterMatchType = filter.FilterMatchType
            }),
            CanAddFilter = canAddFilter
        });
    }

    [HttpPost("filter")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add(AddFilterRequestModel requestModel)
        => await _filterService.AddAsync(
            requestModel.Name,
            requestModel.PropertyAliases,
            requestModel.FilterMatchType,
            requestModel.PrimitiveFieldType)
            ? Ok()
            : BadRequest();

    [HttpPut("filter/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, UpdateFilterRequestModel requestModel)
        => await _filterService.UpdateAsync(
            id,
            requestModel.Name,
            requestModel.PropertyAliases)
            ? Ok()
            : BadRequest();

    [HttpDelete("filter/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id)
        => await _filterService.DeleteAsync(id)
            ? Ok()
            : BadRequest();
}
