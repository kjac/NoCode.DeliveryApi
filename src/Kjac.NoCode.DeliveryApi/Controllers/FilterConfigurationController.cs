using Asp.Versioning;
using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Services;
using Kjac.NoCode.DeliveryApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core;

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
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add(AddFilterRequestModel requestModel)
    {
        Attempt<OperationStatus> result = await _filterService.AddAsync(
            requestModel.Name,
            requestModel.PropertyAliases,
            requestModel.FilterMatchType,
            requestModel.PrimitiveFieldType);

        return OperationStatusResult(result.Result);
    }

    [HttpPut("filter/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, UpdateFilterRequestModel requestModel)
    {
        Attempt<OperationStatus> result = await _filterService.UpdateAsync(
            id,
            requestModel.Name,
            requestModel.PropertyAliases);

        return OperationStatusResult(result.Result);
    }

    [HttpDelete("filter/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id)
    {
        Attempt<OperationStatus> result = await _filterService.DeleteAsync(id);

        return OperationStatusResult(result.Result);
    }
}
