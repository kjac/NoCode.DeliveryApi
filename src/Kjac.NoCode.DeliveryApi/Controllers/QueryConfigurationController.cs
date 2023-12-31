using Kjac.NoCode.DeliveryApi.Models;
using Kjac.NoCode.DeliveryApi.Services;
using Kjac.NoCode.DeliveryApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;

namespace Kjac.NoCode.DeliveryApi.Controllers;

[PluginController(Constants.BackOfficeArea)]
public sealed class QueryConfigurationController : UmbracoAuthorizedJsonController
{
    private readonly IFilterService _filterService;
    private readonly ISortService _sortService;

    public QueryConfigurationController(IFilterService filterService, ISortService sortService)
    {
        _filterService = filterService;
        _sortService = sortService;
    }

    [HttpGet]
    public async Task<IActionResult> All()
    {
        IEnumerable<FilterModel> filters = await _filterService.GetAllAsync();
        IEnumerable<SortModel> sorts = await _sortService.GetAllAsync();

        return Ok(new OverviewViewModel
        {
            Filters = filters.Select(filter => new FilterViewModel
            {
                Key = filter.Key,
                Name = filter.Name,
                Alias = filter.Alias,
                FieldName = filter.IndexFieldName,
                PrimitiveFieldType = filter.PrimitiveFieldType,
                PropertyAliases = filter.PropertyAliases,
                FilterMatchType = filter.FilterMatchType
            }),
            Sorts = sorts.Select(sort => new SortViewModel
            {
                Key = sort.Key,
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

    [HttpPost]
    public async Task<IActionResult> AddFilter(AddFilterRequestModel requestModel)
        => await _filterService.AddAsync(
            requestModel.Name,
            requestModel.PropertyAliases,
            requestModel.FilterMatchType,
            requestModel.PrimitiveFieldType)
            ? Ok()
            : BadRequest();

    [HttpPut]
    public async Task<IActionResult> UpdateFilter(UpdateFilterRequestModel requestModel)
        => await _filterService.UpdateAsync(
            requestModel.Key,
            requestModel.Name,
            requestModel.PropertyAliases)
            ? Ok()
            : BadRequest();

    [HttpPost]
    public async Task<IActionResult> AddSort(AddSortRequestModel requestModel)
        => await _sortService.AddAsync(
            requestModel.Name,
            requestModel.PropertyAlias,
            requestModel.PrimitiveFieldType)
            ? Ok()
            : BadRequest();

    [HttpPut]
    public async Task<IActionResult> UpdateSort(UpdateSortRequestModel requestModel)
        => await _sortService.UpdateAsync(
            requestModel.Key,
            requestModel.Name,
            requestModel.PropertyAlias)
            ? Ok()
            : BadRequest();

    [HttpDelete]
    public async Task<IActionResult> DeleteFilter(Guid key)
        => await _filterService.DeleteAsync(key)
        ? Ok()
        : BadRequest();

    [HttpDelete]
    public async Task<IActionResult> DeleteSort(Guid key)
        => await _sortService.DeleteAsync(key)
            ? Ok()
            : BadRequest();
}
