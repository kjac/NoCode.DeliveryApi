using Kjac.NoCode.DeliveryApi.Services;
using Kjac.NoCode.DeliveryApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;

namespace Kjac.NoCode.DeliveryApi.Controllers;

[PluginController(Constants.BackOfficeArea)]
public sealed class ConfigurationController : UmbracoAuthorizedJsonController
{
    private readonly IFilterService _filterService;
    private readonly ISortService _sortService;

    public ConfigurationController(IFilterService filterService, ISortService sortService)
    {
        _filterService = filterService;
        _sortService = sortService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var filters = await _filterService.GetAllAsync();
        var sorts = await _sortService.GetAllAsync();
        
        return Ok(new AllItemsViewModel
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
    {
        var result = await _filterService.AddAsync(requestModel.Name, requestModel.PropertyAliases, requestModel.FilterMatchType, requestModel.PrimitiveFieldType);
        return result ? Ok() : BadRequest();
    }
    
    [HttpPut]
    public async Task<IActionResult>  UpdateFilter(UpdateFilterRequestModel requestModel)
    {
        var result = await _filterService.UpdateAsync(requestModel.Key, requestModel.Name, requestModel.PropertyAliases);
        return result ? Ok() : BadRequest();
    }

    [HttpPost]
    public async Task<IActionResult> AddSort(AddSortRequestModel requestModel)
    {
        var result = await _sortService.AddAsync(requestModel.Name, requestModel.PropertyAlias, requestModel.PrimitiveFieldType);
        return result ? Ok() : BadRequest();
    }
    
    [HttpPut]
    public async Task<IActionResult> UpdateSort(UpdateSortRequestModel requestModel)
    {
        var result = await _sortService.UpdateAsync(requestModel.Key, requestModel.Name, requestModel.PropertyAlias);
        return result ? Ok() : BadRequest();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteFilter(Guid key)
    {
        var result = await _filterService.DeleteAsync(key);
        return result ? Ok() : BadRequest();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteSort(Guid key)
    {
        var result = await _sortService.DeleteAsync(key);
        return result ? Ok() : BadRequest();
    }
}
