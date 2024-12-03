using Asp.Versioning;
using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Services;
using Kjac.NoCode.DeliveryApi.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Kjac.NoCode.DeliveryApi.Controllers;

[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = "Querying")]
public sealed class QueryConfigurationController : NoCodeDeliveryApiControllerBase
{
    private readonly IFilterService _filterService;
    private readonly ISortService _sortService;

    public QueryConfigurationController(IFilterServiceWithExport filterService, ISortServiceWithExport sortService)
    {
        _filterService = filterService;
        _sortService = sortService;
    }

    [HttpGet("query")]
    [ProducesResponseType<OverviewViewModel>(StatusCodes.Status200OK)]
    public async Task<IActionResult> All()
    {
        IEnumerable<FilterModel> filters = await _filterService.GetAllAsync();
        IEnumerable<SortModel> sorts = await _sortService.GetAllAsync();

        return Ok(new OverviewViewModel
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
            Sorts = sorts.Select(sort => new SortViewModel
            {
                Id = sort.Key,
                Name = sort.Name,
                Alias = sort.Alias,
                FieldName = sort.IndexFieldName,
                PrimitiveFieldType = sort.PrimitiveFieldType,
                PropertyAlias = sort.PropertyAlias
            }),
            CanAddFilter = _filterService.CanAdd(),
            CanAddSort = _sortService.CanAdd()
        });
    }

    [HttpPost("query/filter")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddFilter(AddFilterRequestModel requestModel)
        => await _filterService.AddAsync(
            requestModel.Name,
            requestModel.PropertyAliases,
            requestModel.FilterMatchType,
            requestModel.PrimitiveFieldType)
            ? Ok()
            : BadRequest();

    [HttpPut("query/filter/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateFilter(Guid id, UpdateFilterRequestModel requestModel)
        => await _filterService.UpdateAsync(
            id,
            requestModel.Name,
            requestModel.PropertyAliases)
            ? Ok()
            : BadRequest();

    [HttpPost("query/sort")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> AddSort(AddSortRequestModel requestModel)
        => await _sortService.AddAsync(
            requestModel.Name,
            requestModel.PropertyAlias,
            requestModel.PrimitiveFieldType)
            ? Ok()
            : BadRequest();

    [HttpPut("query/sort/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateSort(Guid id, UpdateSortRequestModel requestModel)
        => await _sortService.UpdateAsync(
            id,
            requestModel.Name,
            requestModel.PropertyAlias)
            ? Ok()
            : BadRequest();

    [HttpDelete("query/filter/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteFilter(Guid id)
        => await _filterService.DeleteAsync(id)
        ? Ok()
        : BadRequest();

    [HttpDelete("query/sort/{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> DeleteSort(Guid id)
        => await _sortService.DeleteAsync(id)
            ? Ok()
            : BadRequest();
}
