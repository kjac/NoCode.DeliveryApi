using Asp.Versioning;
using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Services;
using Kjac.NoCode.DeliveryApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add(AddSortRequestModel requestModel)
        => await _sortService.AddAsync(
            requestModel.Name,
            requestModel.PropertyAlias,
            requestModel.PrimitiveFieldType)
            ? Ok()
            : BadRequest();

    [HttpPut("sort/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, UpdateSortRequestModel requestModel)
        => await _sortService.UpdateAsync(
            id,
            requestModel.Name,
            requestModel.PropertyAlias)
            ? Ok()
            : BadRequest();

    [HttpDelete("sort/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete(Guid id)
        => await _sortService.DeleteAsync(id)
            ? Ok()
            : BadRequest();
}
