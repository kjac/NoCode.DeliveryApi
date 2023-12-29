﻿using Kjac.NoCode.DeliveryApi.Deployment;
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
    private readonly IDeployService _deployService;

    public QueryConfigurationController(IFilterService filterService, ISortService sortService, IDeployService deployService)
    {
        _filterService = filterService;
        _sortService = sortService;
        _deployService = deployService;
    }

    [HttpGet]
    public async Task<IActionResult> All()
    {
        var filters = await _filterService.GetAllAsync();
        var sorts = await _sortService.GetAllAsync();
        
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
        => await ExportOnChange(async () => await _filterService.AddAsync(
            requestModel.Name,
            requestModel.PropertyAliases,
            requestModel.FilterMatchType,
            requestModel.PrimitiveFieldType));
    
    [HttpPut]
    public async Task<IActionResult>  UpdateFilter(UpdateFilterRequestModel requestModel)
        => await ExportOnChange(async () => await _filterService.UpdateAsync(
            requestModel.Key,
            requestModel.Name,
            requestModel.PropertyAliases));

    [HttpPost]
    public async Task<IActionResult> AddSort(AddSortRequestModel requestModel)
        => await ExportOnChange(async () => await _sortService.AddAsync(
            requestModel.Name,
            requestModel.PropertyAlias,
            requestModel.PrimitiveFieldType));

    [HttpPut]
    public async Task<IActionResult> UpdateSort(UpdateSortRequestModel requestModel)
        => await ExportOnChange(async () => await _sortService.UpdateAsync(
            requestModel.Key,
            requestModel.Name,
            requestModel.PropertyAlias));

    [HttpDelete]
    public async Task<IActionResult> DeleteFilter(Guid key)
        => await ExportOnChange(async () => await _filterService.DeleteAsync(key));

    [HttpDelete]
    public async Task<IActionResult> DeleteSort(Guid key)
        => await ExportOnChange(async () => await _sortService.DeleteAsync(key));

    // NOTE: this must be moved to an event handler eventually (to support load balancing), so we'll just handle it quick and dirty for now
    private async Task<IActionResult> ExportOnChange(Func<Task<bool>> action)
    {
        var result = await action();
        if (result)
        {
            await _deployService.ExportAsync();
        }

        return result ? Ok() : BadRequest();
    }
}
