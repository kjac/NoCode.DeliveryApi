using Asp.Versioning;
using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Services;
using Kjac.NoCode.DeliveryApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Core;

namespace Kjac.NoCode.DeliveryApi.Controllers;

[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Sorters")]
public class SortConfigurationController : NoCodeDeliveryApiControllerBase
{
    private readonly ISortService _sortService;

    public SortConfigurationController(ISortService sortService)
        => _sortService = sortService;

    [HttpGet("sort")]
    [ProducesResponseType<SortListViewModel>(StatusCodes.Status200OK)]
    public async Task<IActionResult> All()
    {
        IEnumerable<SortModel> sorts = await _sortService.GetAllAsync();
        var canAddSort = _sortService.CanAdd();
        return Ok(new SortListViewModel
        {
            Sorts = sorts.Select(sort => new SortViewModel
            {
                Id = sort.Key,
                Name = sort.Name,
                Alias = sort.Alias,
                FieldName = sort.IndexFieldName,
                PrimitiveFieldType = sort.PrimitiveFieldType,
                PropertyAlias = sort.PropertyAlias
            }),
            CanAddSort = canAddSort
        });
    }

    [HttpPost("sort")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add(AddSortRequestModel requestModel)
    {
        Attempt<OperationStatus> result = await _sortService.AddAsync(
            requestModel.Name,
            requestModel.PropertyAlias,
            requestModel.PrimitiveFieldType);

        return OperationStatusResult(result.Result);
    }

    [HttpPut("sort/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, UpdateSortRequestModel requestModel)
    {
        Attempt<OperationStatus> result = await _sortService.UpdateAsync(
            id,
            requestModel.Name,
            requestModel.PropertyAlias);

        return OperationStatusResult(result.Result);
    }

    [HttpDelete("sort/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id)
    {
        Attempt<OperationStatus> result = await _sortService.DeleteAsync(id);

        return OperationStatusResult(result.Result);
    }
}
